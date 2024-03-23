namespace ChainflipInsights.Consumers.Telegram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using ChainflipInsights.Feeders.BrokerOverview;
    using ChainflipInsights.Feeders.CexMovement;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.StakedFlip;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Feeders.SwapLimits;
    using ChainflipInsights.Infrastructure.Pipelines;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Humanizer;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class FullTelegramConsumer : IConsumer
    {
        private readonly ILogger<FullTelegramConsumer> _logger;
        private readonly BotConfiguration _configuration;
        private readonly TelegramBotClient _telegramClient;
        private readonly Dictionary<string,string> _brokers;

        public FullTelegramConsumer(
            ILogger<FullTelegramConsumer> logger,
            IOptions<BotConfiguration> options,
            TelegramBotClient telegramClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _telegramClient = telegramClient ?? throw new ArgumentNullException(nameof(telegramClient));
            
            _brokers = _configuration
                .Brokers
                .ToDictionary(
                    x => x.Address,
                    x => x.Name);
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
                    if (!_configuration.EnableTelegram.Value)
                        return;

                    if (input.SwapInfo != null)
                        ProcessSwap(input.SwapInfo, cancellationToken);
                    
                    if (input.IncomingLiquidityInfo != null)
                        ProcessIncomingLiquidityInfo(input.IncomingLiquidityInfo, cancellationToken);
                    
                    if (input.EpochInfo != null)
                        ProcessEpochInfo(input.EpochInfo, cancellationToken);
                    
                    if (input.FundingInfo != null)
                        ProcessFundingInfo(input.FundingInfo, cancellationToken);
                    
                    if (input.RedemptionInfo != null)
                        ProcessRedemptionInfo(input.RedemptionInfo, cancellationToken);
                    
                    if (input.CexMovementInfo != null)
                        ProcessCexMovementInfo(input.CexMovementInfo, cancellationToken);
                    
                    if (input.SwapLimitsInfo != null)
                        ProcessSwapLimitsInfo(input.SwapLimitsInfo, cancellationToken);
                    
                    if (input.PastVolumeInfo != null)
                        ProcessPastVolumeInfo(input.PastVolumeInfo, cancellationToken);
                    
                    if (input.StakedFlipInfo != null)
                        ProcessStakedFlipInfo(input.StakedFlipInfo, cancellationToken);
                    
                    if (input.BrokerOverviewInfo != null)
                        ProcessBrokerOverviewInfo(input.BrokerOverviewInfo, cancellationToken);
                    
                    if (input.BigStakedFlipInfo != null)
                        ProcessBigStakedFlipInfo(input.BigStakedFlipInfo, cancellationToken);
                    
                    Task
                        .Delay(1500, cancellationToken)
                        .GetAwaiter()
                        .GetResult();
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = cancellationToken
                });

            logging.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Telegram Logging completed, {Status}",
                    task.Status),
                cancellationToken);

            return logging;
        }

        private void ProcessSwap(
            SwapInfo swap,
            CancellationToken cancellationToken)
        {
            if (swap.DepositValueUsd < _configuration.DiscordSwapAmountThreshold && 
                !_configuration.SwapWhitelist.Contains(swap.DestinationAsset, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation(
                    "Swap did not meet threshold (${Threshold}) for Full Telegram: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.DiscordSwapAmountThreshold,
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");
                
                return;
            }

            try
            {
                var brokerExists = _brokers.TryGetValue(swap.Broker ?? string.Empty, out var broker);
                
                _logger.LogInformation(
                    "Announcing Swap on Full Telegram: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker}{Broker} -> {ExplorerUrl}",
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    brokerExists ? $" @ {broker}" : string.Empty,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                var text =
                    $"{swap.Emoji} Swapped " +
                    $"**{swap.DepositAmountFormatted} {swap.SourceAsset}** (*${swap.DepositValueUsdFormatted}*) → " +
                    $"**{swap.EgressAmountFormatted} {swap.DestinationAsset}** (*${swap.EgressValueUsdFormatted}*) " +
                    $"{(brokerExists ? $"@ **{broker}** " : string.Empty)}" +
                    $"// **[view swap on explorer]({_configuration.ExplorerSwapsUrl}{swap.Id})**";

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Swap {SwapId} on Full Telegram as Message {MessageId}",
                    swap.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }

        private void ProcessIncomingLiquidityInfo(
            IncomingLiquidityInfo liquidity,
            CancellationToken cancellationToken)
        {
            if (liquidity.DepositValueUsd < _configuration.DiscordLiquidityAmountThreshold)
            {
                _logger.LogInformation(
                    "Incoming Liquidity did not meet threshold (${Threshold}) for Full Telegram: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    _configuration.DiscordLiquidityAmountThreshold,
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");
                
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Incoming Liquidity on Full Telegram: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                var text =
                    $"💵 **Liquidity Added**! An extra " +
                    $"**{liquidity.DepositAmountFormatted} {liquidity.SourceAsset}** (*${liquidity.DepositValueUsdFormatted}*) is available! " +
                    $"// **[view incoming liquidity on explorer]({_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId})**";

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Incoming Liquidity {LiquidityId} on Full Telegram as Message {MessageId}",
                    liquidity.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }

        private void ProcessEpochInfo(
            EpochInfo epoch,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Full Telegram. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Full Telegram -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                var text =
                    $"⏰ **Epoch {epoch.Id} Started**! Current Minimum Active Bid is **{epoch.MinimumBondFormatted} FLIP**. " +
                    $"In total we have **{epoch.TotalBondFormatted}** FLIP bonded, with a maximum bid of **{epoch.MaxBidFormatted} FLIP**. " +
                    $"Last Epoch distributed **{epoch.PreviousEpoch.TotalRewardsFormatted}** FLIP as rewards. " +
                    $"// **[view authority set on explorer]({_configuration.ExplorerAuthorityUrl}{epoch.Id})**";

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Full Telegram as Message {MessageId}",
                    epoch.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }

        private void ProcessFundingInfo(
            FundingInfo funding,
            CancellationToken cancellationToken)
        {
            if (funding.AmountConverted < _configuration.DiscordFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet threshold (${Threshold}) for Full Telegram: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DiscordFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Full Telegram: {Validator} added {Amount} FLIP -> {EpochUrl}",
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

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Full Telegram as Message {MessageId}",
                    funding.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }

        private void ProcessRedemptionInfo(
            RedemptionInfo redemption,
            CancellationToken cancellationToken)
        {
            if (redemption.AmountConverted < _configuration.DiscordRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet threshold (${Threshold}) for Full Telegram: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DiscordRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Full Telegram: {Validator} redeemed {Amount} FLIP -> {EpochUrl}",
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

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Full Telegram as Message {MessageId}",
                    redemption.Id,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }

        private void ProcessCexMovementInfo(
            CexMovementInfo cexMovement,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Full Telegram. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing CEX Movements for {Date} on Full Telegram: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);

                var text =
                    $"🔀 CEX Movements for **{cexMovement.Date:yyyy-MM-dd}** are in! " +
                    $"**{cexMovement.MovementInFormatted} FLIP** moved towards CEX, **{cexMovement.MovementOutFormatted} FLIP** moved towards DEX. " +
                    $"In total, **{(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "CEX" : "DEX" )}** gained **{cexMovement.TotalMovementFormatted} FLIP** {(cexMovement.NetMovement == Feeders.CexMovement.NetMovement.MoreTowardsCex ? "🔴" : "🟢" )}";

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing CEX Movements {Day} on Full Telegram as Message {MessageId}",
                    cexMovement.DayOfYear,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }

        private void ProcessSwapLimitsInfo(
            SwapLimitsInfo swapLimits, 
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordSwapLimitsEnabled.Value)
            {
                _logger.LogInformation(
                    "Swap Limits disabled for Full Telegram: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Swap Limits on Full Telegram: {Limits}",
                    string.Join(", ", swapLimits.SwapLimits.Select(x => $"{x.Asset.Ticker}: {x.SwapLimit}")));

                var text =
                    $"🫡 Swap Limits have changed! " +
                    $"The new limits are: " +
                    $"{string.Join(", ", swapLimits.SwapLimits.Select(x => $"**{x.SwapLimit} {x.Asset.Ticker}**"))}";
                
                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Swap Limits on Full Telegram as Message {MessageId}",
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            } 
        }

        private void ProcessPastVolumeInfo(
            PastVolumeInfo pastVolume,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordPastVolumeEnabled.Value)
            {
                _logger.LogInformation(
                    "Past Volume disabled for Full Telegram. {Date}: {Pairs} Past 24h Volume pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Volume for {Date} on Full Telegram: {Pairs} Past 24h Volume Pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);

                var totalVolume = pastVolume
                    .VolumePairs
                    .Sum(x => x.Value.Value);
                
                var totalFees = pastVolume
                    .VolumePairs
                    .Sum(x => x.Value.Fees);

                var text =
                    $"📊 On **{pastVolume.Date}** we had a volume of " +
                    $"**${totalVolume.ToMetric(decimals: 2)}** and **${totalFees.ToMetric(decimals: 2)}** in fees!";
                
                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Volume on Full Telegram as Message {MessageId}",
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            } 
        }
        
        private void ProcessStakedFlipInfo(
            StakedFlipInfo stakedFlip,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordStakedFlipEnabled.Value)
            {
                _logger.LogInformation(
                    "Staked Flip disabled for Full Telegram. {Date}: {MovementIn} FLIP staked, {MovementOut} FLIP unstaked, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Staked Flip for {Date} on Full Telegram: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    stakedFlip.StakedFormatted,
                    stakedFlip.UnstakedFormatted,
                    stakedFlip.TotalMovementFormatted,
                    stakedFlip.NetMovement);

                var text =
                    $"🏦 stFLIP Movements for **{stakedFlip.Date:yyyy-MM-dd}** are in! " +
                    $"**{stakedFlip.StakedFormatted} FLIP** got staked, **{stakedFlip.UnstakedFormatted} FLIP** got unstaked. " +
                    $"In total, **{stakedFlip.TotalMovementFormatted} FLIP** was **{(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreStaked ? "staked" : "unstaked" )}** {(stakedFlip.NetMovement == Feeders.StakedFlip.NetMovement.MoreUnstaked ? "🔴" : "🟢" )}";

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Staked Flip {Date} on Full Telegram as Message {MessageId}",
                    stakedFlip.Date.ToString("yyyy-MM-dd"),
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
        
        private void ProcessBrokerOverviewInfo(
            BrokerOverviewInfo brokerOverview,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordBrokerOverviewEnabled.Value)
            {
                _logger.LogInformation(
                    "Broker Overview disabled for Full Telegram. {Date}: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Broker Overview for {Date} on Full Telegram: {Brokers} top brokers.",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    brokerOverview.Brokers.Count);

                var text = new StringBuilder();
                text.AppendLine($"🏭 Top Brokers for **{brokerOverview.Date:yyyy-MM-dd}** are in!");

                var emojis = new[]
                {
                    "🥇",
                    "🥈",
                    "🥉",
                    "🏅",
                    "🏅"
                };

                for (var i = 0; i < brokerOverview.Brokers.Count; i++)
                {
                    var brokerInfo = brokerOverview.Brokers[i];
                    var brokerExists = _brokers.TryGetValue(brokerInfo.Ss58, out var broker);
                    var name = brokerExists ? broker : brokerInfo.Ss58;

                    text.AppendLine($"{emojis[i]} **{name}** (**${brokerInfo.VolumeFormatted}** Volume, **${brokerInfo.FeesFormatted}** Fees)");
                }
                
                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text.ToString(),
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Broker Overview {Date} on Full Telegram as Message {MessageId}",
                    brokerOverview.Date.ToString("yyyy-MM-dd"),
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
        
        private void ProcessBigStakedFlipInfo(
            BigStakedFlipInfo bigStakedFlipInfo,
            CancellationToken cancellationToken)
        {
            if (bigStakedFlipInfo.Staked < _configuration.DiscordStakedFlipAmountThreshold)
            {
                _logger.LogInformation(
                    "Staked flip did not meet threshold ({Threshold} FLIP) for Full Telegram: {Amount} FLIP",
                    _configuration.DiscordStakedFlipAmountThreshold,
                    bigStakedFlipInfo.StakedFormatted);
                
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing staked flip on Full Telegram: {Amount} FLIP -> {ExplorerUrl}",
                    bigStakedFlipInfo.StakedFormatted,
                    $"{_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}");

                var text =
                    $"🔥 **Big stFLIP Alert**! " +
                    $"**{bigStakedFlipInfo.StakedFormatted} FLIP** just got staked! " +
                    $"// **[view transaction on explorer]({_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash})**";
                
                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing staked flip {TransactionHash} on Full Telegram as Message {MessageId}",
                    bigStakedFlipInfo.TransactionHash,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
    }
}