namespace ChainflipInsights.Feeders.StakedFlip
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class StakedFlipFeeder : IFeeder
    {
        private const string StakeQuery = 
            """
            {
                stuff: mints(orderBy: blockTimestamp, where: { 
                    blockTimestamp_gte: TIME_FROM,
                    blockTimestamp_lt: TIME_TO
                }) {
                    blockTimestamp
                    amount
                }
            }
            """;
        
        private const string UnstakeQuery = 
            """
            {
                stuff: burns(orderBy: blockTimestamp, where: { 
                    blockTimestamp_gte: TIME_FROM,
                    blockTimestamp_lt: TIME_TO
                }) {
                    blockTimestamp
                    amount
                }
            }
            """;

        private readonly ILogger<StakedFlipFeeder> _logger;
        private readonly Pipeline<StakedFlipInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public StakedFlipFeeder(
            ILogger<StakedFlipFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<StakedFlipInfo> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }
        
        public async Task Start()
        {
            try
            {
                if (!_configuration.EnableStakedFlip.Value)
                {
                    _logger.LogInformation(
                        "Staked Flip not enabled. Skipping {TaskName}",
                        nameof(StakedFlipFeeder));

                    return;
                }

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(StakedFlipFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching StakedFlip Info
                await BackfillStakedFlipInfo(_pipeline.CancellationToken);
                //await ProvideStakedFlipInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(StakedFlipFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(StakedFlipFeeder));
            }
        }

        private async Task ProvideStakedFlipInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastStakedFlip = await GetLastStakedFlip(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var yesterday = DateTime.UtcNow.Date.AddDays(-1);
                var dateString = yesterday.ToString("yyyy-MM-dd");
                if (dateString == lastStakedFlip)
                {
                    _logger.LogInformation(
                        "No new staked flip to announce. Last date is still {StakedFlip}",
                        lastStakedFlip);
                    
                    await Task.Delay(_configuration.StakedFlipQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                var stakedFlipInfo = await GetStakedFlip(
                    yesterday,
                    yesterday.AddDays(1),
                    cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                if (stakedFlipInfo == null)
                {
                    await Task.Delay(_configuration.StakedFlipQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var unstakedFlipInfo = await GetUnstakedFlip(
                    yesterday,
                    yesterday.AddDays(1),
                    cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                if (unstakedFlipInfo == null)
                {
                    await Task.Delay(_configuration.StakedFlipQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var stakedFlip = new StakedFlipInfo(
                    yesterday, 
                    stakedFlipInfo.Data.Data,
                    unstakedFlipInfo.Data.Data);

                _logger.LogInformation(
                    "Broadcasting Staked Flip for {Date}, {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);
                
                await _pipeline.Source.SendAsync(
                    stakedFlip, 
                    cancellationToken);
                
                lastStakedFlip = dateString;
                await StoreLastStakedFlip(lastStakedFlip);
                
                await Task.Delay(_configuration.StakedFlipQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task BackfillStakedFlipInfo(CancellationToken cancellationToken)
        {
            var lastStakedFlip = new DateTime(2023, 11, 23, 0, 0, 0, DateTimeKind.Utc);
            var finalDate = new DateTime(2024, 04, 27, 0, 0, 0, DateTimeKind.Utc);

            while (lastStakedFlip < finalDate)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
      
                var stakedFlipInfo = await GetStakedFlip(
                    lastStakedFlip,
                    lastStakedFlip.AddDays(1),
                    cancellationToken);
                
                var unstakedFlipInfo = await GetUnstakedFlip(
                    lastStakedFlip,
                    lastStakedFlip.AddDays(1),
                    cancellationToken);
                
                var stakedFlip = new StakedFlipInfo(
                    lastStakedFlip, 
                    stakedFlipInfo.Data.Data,
                    unstakedFlipInfo.Data.Data);

                _logger.LogInformation(
                    "Broadcasting Staked Flip for {Date}, {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);
                
                await _pipeline.Source.SendAsync(
                    stakedFlip, 
                    cancellationToken);

                lastStakedFlip = lastStakedFlip.AddDays(1);
                await Task.Delay(1000, cancellationToken);
            }
        }
              
        private async Task<string> GetLastStakedFlip(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastStakedFlipLocation))
                return await File.ReadAllTextAsync(_configuration.LastStakedFlipLocation, cancellationToken);

            await using var file = File.CreateText(_configuration.LastStakedFlipLocation);
            await file.WriteAsync("2024-03-20");
            return "2024-03-20";
        }
        
        private async Task StoreLastStakedFlip(string lastStakedFlip)
        {
            await using var file = File.CreateText(_configuration.LastStakedFlipLocation);
            await file.WriteAsync(lastStakedFlip.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<StakedFlipResponse?> GetStakedFlip(
            DateTimeOffset timeFrom,
            DateTimeOffset timeTo,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("StakedFlipGraph");

                var query = StakeQuery
                    .Replace("TIME_FROM", timeFrom.ToUnixTimeSeconds().ToString())
                    .Replace("TIME_TO", timeTo.ToUnixTimeSeconds().ToString());
                var graphQuery = $"{{ \"query\": \"{query.ReplaceLineEndings("\\n")}\" }}";

                var response = await client.PostAsync(
                    string.Empty,
                    new StringContent(
                        graphQuery,
                        new MediaTypeHeaderValue(MediaTypeNames.Application.Json)),
                    cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response
                        .Content
                        .ReadFromJsonAsync<StakedFlipResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetStakedFlip returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching staked stFLIP failed.");
            }

            return null;
        }
        
        private async Task<StakedFlipResponse?> GetUnstakedFlip(
            DateTimeOffset timeFrom,
            DateTimeOffset timeTo,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("StakedFlipGraph");

                var query = UnstakeQuery
                    .Replace("TIME_FROM", timeFrom.ToUnixTimeSeconds().ToString())
                    .Replace("TIME_TO", timeTo.ToUnixTimeSeconds().ToString());
                var graphQuery = $"{{ \"query\": \"{query.ReplaceLineEndings("\\n")}\" }}";

                var response = await client.PostAsync(
                    string.Empty,
                    new StringContent(
                        graphQuery,
                        new MediaTypeHeaderValue(MediaTypeNames.Application.Json)),
                    cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return await response
                        .Content
                        .ReadFromJsonAsync<StakedFlipResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetUnstakedFlip returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching unstaked stFLIP failed.");
            }

            return null;
        }
    }
}