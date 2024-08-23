namespace ChainflipInsights.Feeders.BigStakedFlip
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
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

    public class BigStakedFlipFeeder : IFeeder
    {
        private const string StakeQuery = 
            """
            {
                stuff: mints(orderBy: blockTimestamp, where: { 
                    blockNumber_gt: LAST_ID
                }) {
                    blockNumber
                    blockTimestamp
                    amount
                    to
                    transactionHash
                }
            }
            """;
        
        private readonly ILogger<BigStakedFlipFeeder> _logger;
        private readonly Pipeline<BigStakedFlipInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public BigStakedFlipFeeder(
            ILogger<BigStakedFlipFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<BigStakedFlipInfo> pipeline)
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
                if (!_configuration.EnableBigStakedFlip.Value)
                {
                    _logger.LogInformation(
                        "Big Staked Flip not enabled. Skipping {TaskName}",
                        nameof(BigStakedFlipFeeder));

                    return;
                }

                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, _configuration.StartupDelay.Value), _pipeline.CancellationToken);
                
                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(BigStakedFlipFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching BigStakedFlip Info
                await ProvideBigStakedFlipInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(BigStakedFlipFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(BigStakedFlipFeeder));
            }
        }
        
        private async Task ProvideBigStakedFlipInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastId = await GetLastBigStakedFlip(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                var stakedInfo = await GetBigStakedFlip(lastId, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (stakedInfo == null)
                {
                    _logger.LogInformation(
                        "Fetching staked flip failed. Last staked flip is still {BlockNumber}",
                        lastId);
                    
                    await Task.Delay(_configuration.BigStakedFlipQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var stakes = stakedInfo
                    .Data.Data
                    .ToList();
                
                if (stakes.Count <= 0)
                {
                    _logger.LogInformation(
                        "No new staked flips to announce. Last staked flip is still {BlockNumber}",
                        lastId);
                    
                    await Task.Delay(_configuration.BigStakedFlipQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                _logger.LogInformation(
                    "{NewStakes} new staked flips to announce.",
                    stakes.Count);
                
                // Stakes are in increasing order
                foreach (var stake in stakes.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
                {
                    try
                    {
                        var stakeInfo = new BigStakedFlipInfo(stake);

                        _logger.LogInformation(
                            "Broadcasting staked flip {BlockNumber}: {Amount} FLIP from {Address} on {Date} -> {EtherScanUrl}",
                            stake.BlockNumber,
                            stakeInfo.StakedFormatted,
                            stakeInfo.To,
                            stakeInfo.Date.ToString("yyyy-MM-dd"),
                            $"{_configuration.EtherScanUrl}{stakeInfo.TransactionHash}");

                        await _pipeline.Source.SendAsync(
                            stakeInfo,
                            cancellationToken);

                        lastId = stake.BlockNumber;
                        await StoreLastBigStakedFlip(lastId);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(
                            e,
                            "Error on broadcasting Big Staked Flip {BlockNumber}",
                            stake.BlockNumber);
                    }
                }
                
                await Task.Delay(_configuration.BigStakedFlipQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<double> GetLastBigStakedFlip(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastBigStakedFlipLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastBigStakedFlipLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastBigStakedFlipLocation);
            await file.WriteAsync("19472885");
            return 19472885;
        }

        private async Task StoreLastBigStakedFlip(double bigStakedFlip)
        {
            await using var file = File.CreateText(_configuration.LastBigStakedFlipLocation);
            await file.WriteAsync(bigStakedFlip.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<BigStakedFlipResponse?> GetBigStakedFlip(
            double fromId,
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("StakedFlipGraph");

                var query = StakeQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
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
                        .ReadFromJsonAsync<BigStakedFlipResponse>(cancellationToken: cancellationToken);
                }

                _logger.LogError(
                    "GetBigStakedFlip returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching big staked flip failed.");
            }
            
            return null;
        }
    }
}