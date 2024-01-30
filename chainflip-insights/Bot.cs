namespace ChainflipInsights
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;
    using ChainflipInsights.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using File = System.IO.File;

    public class Bot
    {
        private const string SUB1K = "🦐";
        private const string SUB2_5K = "🐟";
        private const string SUB5K = "🐙";
        private const string SUB10K = "🦈";
        private const string WHALE = "🐳";
        
        private const string SwapsQuery = 
            """
            {
                allSwaps(orderBy: ID_DESC, first: 500, filter: {
                    id: { greaterThan: LAST_ID }
                 }) {
                    edges {
                        node {
                            id
                            nativeId
                            swapScheduledBlockTimestamp
            
                            depositAmount
                            depositValueUsd
                            sourceAsset
            
                            egressAmount
                            egressValueUsd
                            destinationAsset
                            destinationAddress
            
                            intermediateAmount
                            intermediateValueUsd
                        }
                    }
                }
            }
            """;

        private readonly Dictionary<string, int> _assetDecimals = new()
        {
            { "btc", 8 },
            { "dot", 10 },
            { "eth", 18 },
            { "flip", 18 },
            { "usdc", 6 },
        }; 
        
        private readonly ILogger<Bot> _logger;
        private readonly DiscordSocketClient _discordClient;
        private readonly TelegramBotClient _telegramClient;
        private readonly BotConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public Bot(
            ILogger<Bot> logger,
            IOptions<BotConfiguration> options,
            IHttpClientFactory httpClientFactory,
            DiscordSocketClient discordClient,
            TelegramBotClient telegramClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _telegramClient = telegramClient ?? throw new ArgumentNullException(nameof(telegramClient));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running Bot...");

            await _discordClient.LoginAsync(
                TokenType.Bot,
                _configuration.Token);
            
            await _discordClient.StartAsync();
            
            // Start a loop fetching Swap Info
            await ProvideSwapInfo(cancellationToken);

            cancellationToken.WaitHandle.WaitOne();
            
            if (_discordClient.ConnectionState != ConnectionState.Disconnected)
            {
                await _discordClient.LogoutAsync();
                await _discordClient.StopAsync();
            }
        }
        
        private async Task ProvideSwapInfo(CancellationToken cancellationToken)
        {
            // Give Discord some time to connect
            await Task.Delay(5000, cancellationToken);
            
            var lastId = await GetLastSwapId(cancellationToken);
            
            while (true)
            {
                // Fetch swap info
                var swapsInfo = await GetSwaps(lastId, cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    break;

                if (swapsInfo == null)
                {
                    await Task.Delay(_configuration.SwapInfoDelay.Value, cancellationToken);
                    continue;                    
                }
                
                var swaps = swapsInfo
                    .Data.Data.Data
                    .Select(x => x.Data)
                    .OrderBy(x => x.Id)
                    .ToList();
                
                // Swaps are in increasing order
                foreach (var swap in swaps)
                {
                    await AnnounceSwap(
                        swap, 
                        swaps.Count,
                        cancellationToken);
                    
                    lastId = swap.Id;
                    await StoreLastSwapId(swap.Id);
                }
                
                await Task.Delay(_configuration.SwapInfoDelay.Value, cancellationToken);
            }
        }

        private async Task<double> GetLastSwapId(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastSwapIdLocation))
                return double.Parse(await File.ReadAllTextAsync(_configuration.LastSwapIdLocation, cancellationToken));
            
            await using var file = File.CreateText(_configuration.LastSwapIdLocation);
            await file.WriteAsync("386");
            return 386;
        }

        private async Task StoreLastSwapId(double swapId)
        {
            await using var file = File.CreateText(_configuration.LastSwapIdLocation);
            await file.WriteAsync(swapId.ToString(CultureInfo.InvariantCulture));
        }

        private async Task<SwapsResponse?> GetSwaps(
            double fromId,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient("Graph");

            var query = SwapsQuery.Replace("LAST_ID", fromId.ToString(CultureInfo.InvariantCulture));
            var graphQuery = $"{{ \"query\": \"{query.ReplaceLineEndings("\\n")}\" }}";
            
            var response = await client.PostAsync(
                string.Empty,
                new StringContent(
                    graphQuery, 
                    new MediaTypeHeaderValue(MediaTypeNames.Application.Json)), 
                cancellationToken);

            return await response.Content.ReadFromJsonAsync<SwapsResponse>(cancellationToken: cancellationToken);
        }

        private async Task<SwapResponse?> GetSwap(
            double swapId,
            CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient("Swap");
            
            return await client.GetFromJsonAsync<SwapResponse>(
                $"swaps/{swapId}", 
                cancellationToken: cancellationToken);
        }

        private async Task AnnounceSwap(
            SwapsResponseNode swap,
            int totalSwaps,
            CancellationToken cancellationToken)
        {
            var swapInfo = await GetSwap(swap.Id, cancellationToken);

            var swapStartedAt = DateTimeOffset.FromUnixTimeMilliseconds(swapInfo.DepositReceivedAt);
            var swapFinishedAt = DateTimeOffset.FromUnixTimeMilliseconds(swapInfo.BroadcastSucceededAt);
            var swapTime = swapFinishedAt.Subtract(swapStartedAt);
            
            var inputDecimals = _assetDecimals[swap.SourceAsset.ToLowerInvariant()];
            var inputString = $"0.00{new string('#', inputDecimals - 2)}";
            var swapInput = swap.DepositAmount / Math.Pow(10, inputDecimals);
            
            var outputDecimals = _assetDecimals[swap.DestinationAsset.ToLowerInvariant()];
            var outputString = $"0.00{new string('#', outputDecimals - 2)}";
            var swapOutput = swap.EgressAmount / Math.Pow(10, outputDecimals);

            var dollarString = "0.00";

            var time = DateTimeOffset.Parse(swap.SwapScheduledBlockTimestamp);

            var text =
                $"{GetEmoji(swap.DepositValueUsd)} Swapped " +
                $"**{Math.Round(swapInput, 8).ToString(inputString)} {swap.SourceAsset}** (*${swap.DepositValueUsd.ToString(dollarString)}*) → " +
                $"**{Math.Round(swapOutput, 8).ToString(outputString)} {swap.DestinationAsset}** (*${swap.EgressValueUsd.ToString(dollarString)}*) " +
                // $"in **{HumanTime(swapTime)}** " +
                $"// **[view swap on explorer]({_configuration.ExplorerUrl}{swap.Id})**";
            
            if (_discordClient.ConnectionState == ConnectionState.Connected)
            {
                var infoChannel = (ITextChannel)_discordClient.GetChannel(_configuration.SwapInfoChannelId.Value);

                try {
                await infoChannel.SendMessageAsync(
                    text,
                    flags: MessageFlags.SuppressEmbeds);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Discord meh.");
                }
            }

            try
            {
                var message = await _telegramClient.SendTextMessageAsync(
                    new ChatId(_configuration.TelegramSwapInfoChannelId.Value),
                    text,
                    disableNotification: true,
                    allowSendingWithoutReply: true,
                    cancellationToken: cancellationToken);

                if (totalSwaps > 1)
                    await Task.Delay(1100, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Telegram meh.");
            }

            _logger.LogInformation(
                "Swap {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} at {SwapTime} -> {ExplorerUrl}",
                Math.Round(swapInput, 8).ToString(inputString),
                swap.SourceAsset,
                Math.Round(swapOutput, 8).ToString(outputString),
                swap.DestinationAsset,
                $"{time:yyyy-MM-dd HH:mm:ss}",
                $"{_configuration.ExplorerUrl}{swap.Id}");
        }

        private static string GetEmoji(double amount) =>
            amount switch
            {
                > 10000 => WHALE,
                > 5000 => SUB10K,
                > 2500 => SUB5K,
                > 1000 => SUB2_5K,
                _ => SUB1K
            };

        private static string HumanTime(TimeSpan span)
        {
            var time = new StringBuilder();

            if (span.Hours > 0)
                time.Append($"{span.Hours}h");
            
            if (span.Minutes > 0)
                time.Append($"{span.Minutes}m");

            if (span.Seconds > 0)
                time.Append($"{span.Seconds}s");

            return time.ToString();
        }
    }
    
    public class SwapsResponse
    {
        [JsonPropertyName("data")] 
        public SwapsResponseData Data { get; set; }
    }

    public class SwapsResponseData
    {
        [JsonPropertyName("allSwaps")] 
        public SwapsResponseAllSwaps Data { get; set; }
    }

    public class SwapsResponseAllSwaps
    {
        [JsonPropertyName("edges")] 
        public SwapsResponseEdges[] Data { get; set; }
    }

    public class SwapsResponseEdges
    {
        [JsonPropertyName("node")] 
        public SwapsResponseNode Data { get; set; }
    }

    public class SwapsResponseNode
    {
        // "node": {
        //     "id": 374,
        //     "nativeId": "374",
        //     "swapScheduledBlockTimestamp": "2024-01-29T20:20:54+00:00",
        //     "depositAmount": "2000000000000000000000",
        //     "depositValueUsd": "10020.000000000000000000000000000000",
        //     "sourceAsset": "FLIP",
        //     "egressAmount": "9971679630",
        //     "egressValueUsd": "9971.679630000000000000000000000000",
        //     "destinationAsset": "USDC",
        //     "destinationAddress": "0xc79fb6449c121a8f453eec2bc78bac857711cb1d",
        //     "intermediateAmount": null,
        //     "intermediateValueUsd": null
        // }
        
        [JsonPropertyName("id")] 
        public double Id { get; set; }
        
        [JsonPropertyName("nativeId")] 
        public double NativeId { get; set; }
        
        [JsonPropertyName("swapScheduledBlockTimestamp")] 
        public string SwapScheduledBlockTimestamp { get; set; }
        
        [JsonPropertyName("depositAmount")] 
        public double DepositAmount { get; set; }
        
        [JsonPropertyName("depositValueUsd")] 
        public double DepositValueUsd { get; set; }
        
        [JsonPropertyName("sourceAsset")] 
        public string SourceAsset { get; set; }
        
        [JsonPropertyName("egressAmount")] 
        public double EgressAmount { get; set; }
        
        [JsonPropertyName("egressValueUsd")] 
        public double EgressValueUsd { get; set; }
        
        [JsonPropertyName("destinationAsset")] 
        public string DestinationAsset { get; set; }
        
        [JsonPropertyName("destinationAddress")] 
        public string DestinationAddress { get; set; }
        
        [JsonPropertyName("intermediateAmount")] 
        public double? IntermediateAmount { get; set; }

        [JsonPropertyName("intermediateValueUsd")] 
        public double? IntermediateValueUsd { get; set; }
    }
    
    public class SwapResponse
    {
        [JsonPropertyName("depositReceivedAt")]
        public long DepositReceivedAt { get; set; }
        
        [JsonPropertyName("broadcastSucceededAt")]
        public long BroadcastSucceededAt { get; set; }
    }
}