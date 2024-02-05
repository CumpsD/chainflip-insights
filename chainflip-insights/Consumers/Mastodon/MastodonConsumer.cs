namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.CexMovement;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Mastonet;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MastodonConsumer : IConsumer
    {
        private readonly ILogger<MastodonConsumer> _logger;
        private readonly MastodonClient _mastodonClient;
        private readonly BotConfiguration _configuration;

        public MastodonConsumer(
            ILogger<MastodonConsumer> logger,
            IOptions<BotConfiguration> options,
            MastodonClient mastodonClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mastodonClient = mastodonClient ?? throw new ArgumentNullException(nameof(mastodonClient));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
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
                    if (!_configuration.EnableMastodon.Value)
                        return;

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
                    "Mastodon Logging completed, {Status}",
                    task.Status),
                cancellationToken);

            return logging;
        }

        private void ProcessSwap(SwapInfo swap)
        {
            if (swap.DepositValueUsd < _configuration.MastodonSwapAmountThreshold)
            {
                _logger.LogInformation(
                    "Swap did not meet treshold (${Threshold}) for Mastodon: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.MastodonSwapAmountThreshold,
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");
                
                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Swap on Mastodon: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                var text =
                    $"{swap.Emoji} Swapped {_configuration.ExplorerSwapsUrl}{swap.Id}\n" +
                    $"➡️ {swap.DepositAmountFormatted} #{swap.SourceAsset} (${swap.DepositValueUsdFormatted})\n" +
                    $"⬅️ {swap.EgressAmountFormatted} #{swap.DestinationAsset} (${swap.EgressValueUsdFormatted})\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Swap {SwapId} on Mastodon as Message {MessageId}",
                    swap.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessIncomingLiquidityInfo(IncomingLiquidityInfo liquidity)
        {
            if (liquidity.DepositValueUsd < _configuration.MastodonLiquidityAmountThreshold)
                return;
            
            // TODO: Send
        }

        private void ProcessEpochInfo(EpochInfo epoch)
        {
            if (!_configuration.MastodonEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Mastodon. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Epoch {Epoch} on Mastodon -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                
                var text =
                    $"⏰ Epoch {epoch.Id} Started {_configuration.ExplorerAuthorityUrl}{epoch.Id}\n" +
                    $"➖ Minimum Bid is {epoch.MinimumBondFormatted} #FLIP\n" +
                    $"➕ Maximum Bid is {epoch.MaxBidFormatted} #FLIP\n" +
                    $"🧮 Total bonded is {epoch.TotalBondFormatted} #FLIP\n" +
                    $"💰 Last Epoch distributed {epoch.PreviousEpoch.TotalRewardsFormatted} #FLIP in rewards\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Epoch {EpochId} on Mastodon as Message {MessageId}",
                    epoch.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessFundingInfo(FundingInfo funding)
        {
            if (funding.AmountConverted < _configuration.MastodonFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet treshold (${Threshold}) for Mastodon: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.MastodonFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Mastodon: {Validator} added {Amount} FLIP -> {EpochUrl}",
                    funding.Id,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                
                var text =
                    $"🪙 Validator {funding.Validator} added {funding.AmountFormatted} #FLIP! {string.Format(_configuration.ValidatorUrl, funding.ValidatorName)}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Mastodon as Message {MessageId}",
                    funding.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessRedemptionInfo(RedemptionInfo redemption)
        {
            if (redemption.AmountConverted < _configuration.MastodonRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet treshold (${Threshold}) for Mastodon: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.MastodonRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Mastodon: {Validator} redeemed {Amount} #FLIP -> {EpochUrl}",
                    redemption.Id,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                
                var text =
                    $"💸 Validator {redemption.Validator} redeemed {redemption.AmountFormatted} FLIP! {string.Format(_configuration.ValidatorUrl, redemption.ValidatorName)}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing Redemption {RedemptionId} on Mastodon as Message {MessageId}",
                    redemption.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }

        private void ProcessCexMovementInfo(CexMovementInfo cexMovement)
        {
            if (!_configuration.MastodonCexMovementEnabled.Value)
            {
                _logger.LogInformation(
                    "CEX Movement disabled for Mastodon. {Date}: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
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
                    "Announcing CEX Movements for {Date} on Mastodon: {MovementIn} FLIP in, {MovementOut} FLIP out, {Movement} FLIP {NetMovement}.",
                    cexMovement.Date.ToString("yyyy-MM-dd"),
                    cexMovement.MovementInFormatted,
                    cexMovement.MovementOutFormatted,
                    cexMovement.TotalMovementFormatted,
                    cexMovement.NetMovement);
                
                var text =
                    $"🔀 CEX Movements for {cexMovement.Date:yyyy-MM-dd} are in!\n" +
                    $"⬆️ {cexMovement.MovementInFormatted} #FLIP moved towards CEX\n" +
                    $"⬇️ {cexMovement.MovementOutFormatted} #FLIP moved towards DEX\n" +
                    $"{(cexMovement.NetMovement == NetMovement.MoreTowardsCex ? "🔴" : "🟢" )} {(cexMovement.NetMovement == NetMovement.MoreTowardsCex ? "CEX" : "DEX" )} gained {cexMovement.TotalMovementFormatted} #FLIP\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();
                
                _logger.LogInformation(
                    "Announcing CEX Movements {Day} on Mastodon as Message {MessageId}",
                    cexMovement.DayOfYear,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}