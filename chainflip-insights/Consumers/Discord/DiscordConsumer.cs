namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure.Pipelines;
    using global::Discord;
    using global::Discord.WebSocket;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class DiscordConsumer
    {
        private readonly ILogger<DiscordConsumer> _logger;
        private readonly BotConfiguration _configuration;
        private readonly DiscordSocketClient _discordClient;

        public DiscordConsumer(
            ILogger<DiscordConsumer> logger,
            IOptions<BotConfiguration> options,
            DiscordSocketClient discordClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
        }
        
        public ITargetBlock<SwapInfo> Build(
            CancellationToken ct)
        {
            var announcer = BuildAnnouncer(ct);
            return new EncapsulatingTarget<SwapInfo, SwapInfo>(announcer, announcer);
        }

        private ActionBlock<SwapInfo> BuildAnnouncer(
            CancellationToken ct)
        {
            var logging = new ActionBlock<SwapInfo>(
                swap =>
                {
                    if (!_configuration.EnableDiscord.Value)
                        return;

                    if (swap.DepositValueUsd < _configuration.DiscordAmountThreshold)
                        return;

                    VerifyConnection(ct);
                    
                    if (_discordClient.ConnectionState != ConnectionState.Connected)
                        return;
                    
                    try
                    {
                        _logger.LogInformation(
                            "Announcing Swap on Discord: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                            swap.DepositAmountFormatted,
                            swap.SourceAsset,
                            swap.EgressAmountFormatted,
                            swap.DestinationAsset,
                            $"{_configuration.ExplorerUrl}{swap.Id}");
                        
                        var text =
                            $"{swap.Emoji} Swapped " +
                            $"**{swap.DepositAmountFormatted} {swap.SourceAsset}** (*${swap.DepositValueUsdFormatted}*) → " +
                            $"**{swap.EgressAmountFormatted} {swap.DestinationAsset}** (*${swap.EgressValueUsdFormatted}*) " +
                            $"// **[view swap on explorer]({_configuration.ExplorerUrl}{swap.Id})**";

                        var infoChannel = (ITextChannel)_discordClient.GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                        infoChannel
                            .SendMessageAsync(
                                text,
                                flags: MessageFlags.SuppressEmbeds)
                            .GetAwaiter()
                            .GetResult();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Discord meh.");
                    }
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = ct
                });

            logging.Completion.ContinueWith(
                task =>
                {
                    _logger.LogDebug(
                        "Discord Logging completed, {Status}",
                        task.Status);

                    if (_discordClient.ConnectionState == ConnectionState.Disconnected) 
                        return;
                    
                    _logger.LogDebug("Disconnecting Discord client");
                        
                    _discordClient
                        .LogoutAsync()
                        .GetAwaiter()
                        .GetResult();
                        
                    _discordClient
                        .StopAsync()
                        .GetAwaiter()
                        .GetResult();
                },
                ct);

            return logging;
        }

        private void VerifyConnection(CancellationToken cancellationToken)
        {
            if (_discordClient.ConnectionState == ConnectionState.Connected)
                return;
            
            var ready = false;

            Task OnReady()
            {
                ready = true;
                return Task.CompletedTask;
            }
            
            _discordClient.Ready += OnReady;

            _discordClient
                .LoginAsync(
                    TokenType.Bot,
                    _configuration.DiscordToken)
                .GetAwaiter()
                .GetResult();
             
             _discordClient
                 .StartAsync()
                 .GetAwaiter()
                 .GetResult();

             // Hacky workaround to make sure discord is ready before proceeding
             var retry = 0;
             while (!ready && retry < 2)
             {
                 retry++;
                 Task.Delay(1000, cancellationToken);
             }
             
             _discordClient.Ready -= OnReady;
        }
    }
}