namespace ChainflipInsights.Feeders.CfeVersion
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

    public class CfeVersionFeeder
    {
        private const string CfeVersionQuery = 
            """
            {
                allCfeVersions(orderBy: ID_DESC) {
                    edges {
                        node {
                            id
                            validatorsByCfeVersionId {
                                edges {
                                    node {
                                        idSs58
                                        lastHeartbeatBlockId
                                    }
                                }
                            }
                        }
                    }
                }
            }
            """;
        
        private readonly ILogger<CfeVersionFeeder> _logger;
        private readonly Pipeline<CfeVersionInfo> _pipeline;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public CfeVersionFeeder(
            ILogger<CfeVersionFeeder> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            Pipeline<CfeVersionInfo> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }
        
        public async Task Start()
        {
            if (!_configuration.EnableCfeVersion.Value)
            {
                _logger.LogInformation(
                    "CfeVersion not enabled. Skipping {TaskName}",
                    nameof(CfeVersionFeeder));
                
                return;
            }
            
            _logger.LogInformation(
                "Starting {TaskName}",
                nameof(CfeVersionFeeder));

            // Give the consumers some time to connect
            await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);
            
            // Start a loop fetching CfeVersion Info
            await ProvideCfeVersionInfo(_pipeline.CancellationToken);
            
            _logger.LogInformation(
                "Stopping {TaskName}",
                nameof(CfeVersionFeeder));
        }

        private async Task ProvideCfeVersionInfo(CancellationToken cancellationToken)
        {
         
        }
        
        private async Task<string> GetLastCfeVersion(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastCfeVersionLocation))
                return await File.ReadAllTextAsync(_configuration.LastCfeVersionLocation, cancellationToken);
            
            await using var file = File.CreateText(_configuration.LastCfeVersionLocation);
            await file.WriteAsync("1.1.7");
            return "1.1.7";
        }
        
        private async Task StoreLastCfeVersion(string cfeVersion)
        {
            await using var file = File.CreateText(_configuration.LastCfeVersionLocation);
            await file.WriteAsync(cfeVersion.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<CfeVersionResponse?> GetCfeVersion(
            double fromId,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient("Graph");

            var query = CfeVersionQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
            var graphQuery = $"{{ \"query\": \"{query.ReplaceLineEndings("\\n")}\" }}";
            
            var response = await client.PostAsync(
                string.Empty,
                new StringContent(
                    graphQuery, 
                    new MediaTypeHeaderValue(MediaTypeNames.Application.Json)), 
                cancellationToken);

            return await response.Content.ReadFromJsonAsync<CfeVersionResponse>(cancellationToken: cancellationToken);
        }
    }
}