namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Liquidity;
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

        public ITargetBlock<BroadcastInfo> Build(
            CancellationToken cancellationToken)
        {
            var announcer = BuildAnnouncer(cancellationToken);
            return new EncapsulatingTarget<BroadcastInfo, BroadcastInfo>(announcer, announcer);
        }

        private ActionBlock<BroadcastInfo> BuildAnnouncer(
            CancellationToken cancellationToken)
        {
            var logging = new ActionBlock<BroadcastInfo>(
                input =>
                {
                    if (!_configuration.EnableDiscord.Value)
                        return;
                    
                    VerifyConnection(cancellationToken);

                    if (input.SwapInfo != null)
                        ProcessSwap(input.SwapInfo);
                    
                    if (input.IncomingLiquidityInfo != null)
                        ProcessIncomingLiquidityInfo(input.IncomingLiquidityInfo);
                    
                    if (input.EpochInfo != null)
                        ProcessEpochInfo(input.EpochInfo);
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = cancellationToken
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
                cancellationToken);

            return logging;
        }

        private void ProcessSwap(SwapInfo swap)
        {
            if (swap.DepositValueUsd < _configuration.DiscordSwapAmountThreshold)
            {
                _logger.LogInformation(
                    "Swap did not meet treshold (${Threshold}) for Discord: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.DiscordSwapAmountThreshold,
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");
                
                return;
            }

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
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                var text =
                    $"{swap.Emoji} Swapped " +
                    $"**{swap.DepositAmountFormatted} {swap.SourceAsset}** (*${swap.DepositValueUsdFormatted}*) → " +
                    $"**{swap.EgressAmountFormatted} {swap.DestinationAsset}** (*${swap.EgressValueUsdFormatted}*) " +
                    $"// **[view swap on explorer]({_configuration.ExplorerSwapsUrl}{swap.Id})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Swap {SwapId} on Discord as Message {MessageId}",
                    swap.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }

        private void ProcessIncomingLiquidityInfo(IncomingLiquidityInfo liquidity)
        {
            if (liquidity.DepositValueUsd < _configuration.DiscordLiquidityAmountThreshold)
            {
                _logger.LogInformation(
                    "Incoming Liquidity did not meet treshold (${Threshold}) for Discord: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    _configuration.DiscordLiquidityAmountThreshold,
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");
                
                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;
            
            try
            {
                _logger.LogInformation(
                    "Announcing Incoming Liquidity on Discord: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                var text =
                    $"💵 **Liquidity Added**! An extra " +
                    $"**{liquidity.DepositAmountFormatted} {liquidity.SourceAsset}** (*${liquidity.DepositValueUsdFormatted}*) is available! " +
                    $"// **[view incoming liquidity on explorer]({_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Incoming Liquidity {LiquidityId} on Discord as Message {MessageId}",
                    liquidity.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }

        private void ProcessEpochInfo(EpochInfo epoch)
        {
            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;
            
            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Discord -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                var text =
                    $"⏰ **Epoch {epoch.Id} Started**! Current MAB is **{epoch.MinimumBondFormatted} FLIP**. " +
                    $"In total we have **{epoch.TotalBondFormatted}** FLIP bonded, with a maximum bond of **{epoch.MaxBidFormatted} FLIP**. " +
                    $"Last Epoch distributed **{epoch.TotalRewardsFormatted}** FLIP as rewards." +
                    $"// **[view authority set on explorer]({_configuration.ExplorerAuthorityUrl}{epoch.Id})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Discord as Message {MessageId}",
                    epoch.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
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

            _logger.LogDebug("Requesting Discord connection");

            _discordClient
                .LoginAsync(
                    TokenType.Bot,
                    _configuration.DiscordToken)
                .GetAwaiter()
                .GetResult();

            _logger.LogDebug("Discord logged in");

            _discordClient
                .StartAsync()
                .GetAwaiter()
                .GetResult();

            _logger.LogDebug("Discord started");

            // Hacky workaround to make sure discord is ready before proceeding
            var retry = 0;
            while (!ready && retry < 10)
            {
                _logger.LogDebug("Discord not yet ready");
                retry++;
                Task
                    .Delay(1000, cancellationToken)
                    .GetAwaiter()
                    .GetResult();
            }

            _logger.LogDebug("Discord status: {DiscordStatus}", _discordClient.ConnectionState);

            _discordClient.Ready -= OnReady;
        }
    }
}