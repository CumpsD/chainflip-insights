namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.CexMovement;
    using ChainflipInsights.Feeders.CfeVersion;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Feeders.SwapLimits;
    using ChainflipInsights.Infrastructure.Pipelines;
    using global::Discord;
    using global::Discord.WebSocket;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Semver;

    public class DiscordConsumer : IConsumer
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
                    
                    if (input.FundingInfo != null)
                        ProcessFundingInfo(input.FundingInfo);
                    
                    if (input.RedemptionInfo != null)
                        ProcessRedemptionInfo(input.RedemptionInfo);
                    
                    if (input.CexMovementInfo != null)
                        ProcessCexMovementInfo(input.CexMovementInfo);
                    
                    if (input.CfeVersionInfo != null)
                        ProcessCfeVersionInfo(input.CfeVersionInfo);
                    
                    if (input.SwapLimitsInfo != null)
                        ProcessSwapLimitsInfo(input.SwapLimitsInfo);
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = cancellationToken
                });

            logging.Completion.ContinueWith(
                task =>
                {
                    _logger.LogInformation(
                        "Discord Logging completed, {Status}",
                        task.Status);

                    if (_discordClient.ConnectionState == ConnectionState.Disconnected)
                        return;

                    _logger.LogInformation("Disconnecting Discord client");

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
            if (!_configuration.DiscordEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Discord. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                
                return;
            }
            
            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;
            
            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Discord -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                var text =
                    $"⏰ **Epoch {epoch.Id} Started**! Current Minimum Active Bid is **{epoch.MinimumBondFormatted} FLIP**. " +
                    $"In total we have **{epoch.TotalBondFormatted}** FLIP bonded, with a maximum bid of **{epoch.MaxBidFormatted} FLIP**. " +
                    $"Last Epoch distributed **{epoch.PreviousEpoch.TotalRewardsFormatted}** FLIP as rewards. " +
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

        private void ProcessFundingInfo(FundingInfo funding)
        {
            if (funding.AmountConverted < _configuration.DiscordFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet treshold (${Threshold}) for Discord: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DiscordFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                return;
            }
            
            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;
            
            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Discord: {Validator} added {Amount} FLIP -> {EpochUrl}",
                    funding.Id,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                var validator =
                    string.IsNullOrWhiteSpace(funding.ValidatorAlias)
                        ? $"**`{funding.ValidatorName}`**" 
                        : $"**`{funding.ValidatorName}`** (**{funding.ValidatorAlias}**)";
                
                var text =
                    $"🪙 Validator {validator} added **{funding.AmountFormatted} FLIP**! " +
                    $"// **[view validator on explorer]({string.Format(_configuration.ValidatorUrl, funding.ValidatorName)})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Discord as Message {MessageId}",
                    funding.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }

        private void ProcessRedemptionInfo(RedemptionInfo redemption)
        {
            if (redemption.AmountConverted < _configuration.DiscordRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet treshold (${Threshold}) for Discord: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DiscordRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                return;
            }
            
            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;
            
            try
            {
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Discord: {Validator} redeemed {Amount} FLIP -> {EpochUrl}",
                    redemption.Id,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                var validator =
                    string.IsNullOrWhiteSpace(redemption.ValidatorAlias)
                        ? $"**`{redemption.ValidatorName}`**" 
                        : $"**`{redemption.ValidatorName}`** (**{redemption.ValidatorAlias}**)";
                
                var text =
                    $"💸 Validator {validator} redeemed **{redemption.AmountFormatted} FLIP**! " +
                    $"// **[view validator on explorer]({string.Format(_configuration.ValidatorUrl, redemption.ValidatorName)})**";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Discord as Message {MessageId}",
                    redemption.Id,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }

        private void ProcessCexMovementInfo(CexMovementInfo cexMovement)
        {
            if (!_configuration.DiscordCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Discord. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);
                
                return;
            }
            
            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;
            
            try
            {
                _logger.LogInformation(
                    "Announcing CEX Movements for {Date} on Discord: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                var text =
                    $"🔀 CEX Movements for **{cexMovement.Date:yyyy-MM-dd}** are in! " +
                    $"**{cexMovement.MovementInFormatted} FLIP** moved towards CEX, **{cexMovement.MovementOutFormatted} FLIP** moved towards DEX. " +
                    $"In total, **{(cexMovement.NetMovement == NetMovement.MoreTowardsCex ? "CEX" : "DEX" )}** gained **{cexMovement.TotalMovementFormatted} FLIP** {(cexMovement.NetMovement == NetMovement.MoreTowardsCex ? "🔴" : "🟢" )}";

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing CEX Movements {Day} on Discord as Message {MessageId}",
                    cexMovement.DayOfYear,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }

        private void ProcessCfeVersionInfo(CfeVersionsInfo cfeVersionInfo)
        {
            if (!_configuration.DiscordCfeVersionEnabled.Value)
            {
                _logger.LogInformation(
                    "CFE Version disabled for Discord. {Date}: {Versions} CFE Versions.",
                    cfeVersionInfo.Date,
                    cfeVersionInfo.Versions.Count);
                
                return;
            }
            
            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;
            
            try
            {
                _logger.LogInformation(
                    "Announcing CFE Versions for {Date} on Discord: {Versions} CFE Versions.",
                    cfeVersionInfo.Date,
                    cfeVersionInfo.Versions.Count);

                var maxVersion = cfeVersionInfo.Versions.Keys.Max(x => x);
                var upToDateValidators = cfeVersionInfo
                    .Versions[maxVersion]
                    .Validators
                    .Count(x => x.ValidatorStatus == ValidatorStatus.Online);

                var outdatedValidators = cfeVersionInfo
                    .Versions
                    .Where(x => x.Key < maxVersion && x.Value.Validators.Any(v => v.ValidatorStatus == ValidatorStatus.Online))
                    .ToList();
                
                var text =
                    $"📜 CFE overview for **{cfeVersionInfo.Date}**! " +
                    $"The current version is **{maxVersion}**, which **{upToDateValidators} online validators** are running. " +
                    $"There are **{outdatedValidators.Sum(x => x.Value.Validators.Count(v => v.ValidatorStatus == ValidatorStatus.Online))} online validators** on older versions: " +
                    $"{string.Join(", ", outdatedValidators.Select(x => $"**{x.Value.Validators.Count(v => v.ValidatorStatus == ValidatorStatus.Online)}** on **{x.Key}**"))}";
                
                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);
                
                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing CFE Versions {Day} on Discord as Message {MessageId}",
                    cfeVersionInfo.Date,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }

        private void ProcessSwapLimitsInfo(SwapLimitsInfo swapLimits)
        {
            if (!_configuration.DiscordSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Discord: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));
                
                return;
            }
            
            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;
            
            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Discord: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are: " +
                    $"{string.Join(", ", swapLimits.SwapLimits.Select(x => $"**{x.Asset.Ticker}**: **{x.SwapLimit}**"))}";
                
                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);
                
                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Swap Limits on Discord as Message {MessageId}",
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

            _logger.LogInformation("Requesting Discord connection");

            _discordClient
                .LoginAsync(
                    TokenType.Bot,
                    _configuration.DiscordToken)
                .GetAwaiter()
                .GetResult();

            _logger.LogInformation("Discord logged in");

            _discordClient
                .StartAsync()
                .GetAwaiter()
                .GetResult();

            _logger.LogInformation("Discord started");

            // Hacky workaround to make sure discord is ready before proceeding
            var retry = 0;
            while (!ready && retry < 10)
            {
                _logger.LogInformation("Discord not yet ready");
                retry++;
                Task
                    .Delay(1000, cancellationToken)
                    .GetAwaiter()
                    .GetResult();
            }

            _logger.LogInformation("Discord status: {DiscordStatus}", _discordClient.ConnectionState);

            _discordClient.Ready -= OnReady;
        }
    }
}