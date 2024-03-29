namespace ChainflipInsights.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class PriceProvider
    {
        private const string EthPriceQuery = 
            """
            {
                bundle(id: \"1\") {
                    ethPrice
                }
            }
            """;
        
        private const string FlipPriceQuery = 
            """
            {
              token(id: \"0x826180541412d574cf1336d22c0c0a287822678a\") {
                symbol
                name
                decimals
                derivedETH
              }
            }
            """;
        
        private readonly ILogger<PriceProvider> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        private DateTimeOffset? _lastEthPriceMoment;
        private DateTimeOffset? _lastFlipPriceMoment;
        private double? _lastEthPrice;
        private double? _lastFlipPrice;
        
        public PriceProvider(
            ILogger<PriceProvider> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<double?> GetEthPrice(
            CancellationToken cancellationToken = default)
        {
            // Cache for 5 minutes
            var timeout = DateTimeOffset.UtcNow.AddMinutes(-5);
            if (_lastEthPriceMoment > timeout)
                return _lastEthPrice;

            var ethPrice = await GetEthPriceInternal(cancellationToken);
            if (ethPrice == null)
                return null;

            _lastEthPriceMoment = DateTimeOffset.UtcNow;
            _lastEthPrice = ethPrice;

            return ethPrice;
        }

        public async Task<double?> GetFlipPriceInEth(
            CancellationToken cancellationToken = default)
        {
            // Cache for 5 minutes
            var timeout = DateTimeOffset.UtcNow.AddMinutes(-5);
            if (_lastFlipPriceMoment > timeout)
                return _lastFlipPrice;

            var flipPrice = await GetFlipPriceInternal(cancellationToken);
            if (flipPrice == null)
                return null;

            _lastFlipPriceMoment = DateTimeOffset.UtcNow;
            _lastFlipPrice = flipPrice;

            return flipPrice;
        }

        public async Task<double?> GetFlipPriceInUsd(
            CancellationToken cancellationToken = default)
        {
            var ethPrice = await GetEthPrice(cancellationToken);
            if (ethPrice == null)
                return null;
            
            var flipPriceInEth = await GetFlipPriceInEth(cancellationToken);
            if (flipPriceInEth == null)
                return null;

            return Math.Round(ethPrice.Value * flipPriceInEth.Value, 2);
        }

        private async Task<double?> GetEthPriceInternal(
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("UniswapV2Graph");

                var graphQuery = $"{{ \"query\": \"{EthPriceQuery.ReplaceLineEndings("\\n")}\" }}";

                var response = await client.PostAsync(
                    string.Empty,
                    new StringContent(
                        graphQuery,
                        new MediaTypeHeaderValue(MediaTypeNames.Application.Json)),
                    cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var price = await response
                        .Content
                        .ReadFromJsonAsync<EthPriceResponse>(cancellationToken: cancellationToken);

                    return price?.Data.Data.EthPrice;
                }

                _logger.LogError(
                    "GetEthPrice returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching swaps failed.");
            }
            
            return null;
        }
    
        private async Task<double?> GetFlipPriceInternal(
            CancellationToken cancellationToken)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("UniswapV3Graph");

                var graphQuery = $"{{ \"query\": \"{FlipPriceQuery.ReplaceLineEndings("\\n")}\" }}";

                var response = await client.PostAsync(
                    string.Empty,
                    new StringContent(
                        graphQuery,
                        new MediaTypeHeaderValue(MediaTypeNames.Application.Json)),
                    cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var price = await response
                        .Content
                        .ReadFromJsonAsync<FlipPriceResponse>(cancellationToken: cancellationToken);

                    return price?.Data.Data.DerivedETH;
                }

                _logger.LogError(
                    "GetFlipPrice returned {StatusCode}: {Error}\nRequest: {Request}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    graphQuery);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching swaps failed.");
            }
                
            return null;
        }
    }

    public class FlipPriceResponse
    {
        [JsonPropertyName("data")] 
        public FlipPriceResponseData Data { get; set; }
    }

    public class FlipPriceResponseData
    {
        [JsonPropertyName("token")] 
        public FlipPriceData Data { get; set; }
    }

    public class FlipPriceData
    {
        [JsonPropertyName("derivedETH")] 
        public double DerivedETH { get; set; }
    }

    public class EthPriceResponse
    {
        [JsonPropertyName("data")] 
        public EthPriceResponseData Data { get; set; }
    }

    public class EthPriceResponseData
    {
        [JsonPropertyName("bundle")] 
        public EthPriceData Data { get; set; }
    }

    public class EthPriceData
    {
        [JsonPropertyName("ethPrice")] 
        public double EthPrice { get; set; }
    }
}