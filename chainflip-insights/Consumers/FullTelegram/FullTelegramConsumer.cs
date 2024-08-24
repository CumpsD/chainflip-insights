namespace ChainflipInsights.Consumers.FullTelegram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.EntityFramework;
    using ChainflipInsights.Infrastructure.Pipelines;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public partial class FullTelegramConsumer : IConsumer
    {
        private readonly ILogger<FullTelegramConsumer> _logger;
        private readonly IDbContextFactory<BotContext> _dbContextFactory;
        private readonly BotConfiguration _configuration;
        private readonly TelegramBotClient _telegramClient;
        private readonly Dictionary<string,string> _brokers;

        private readonly ReactionTypeEmoji _tadaEmoji = new() { Emoji = "🎉" };
        private readonly ReactionTypeEmoji _angryEmoji = new() { Emoji = "🤬" };
        
        public FullTelegramConsumer(
            ILogger<FullTelegramConsumer> logger,
            IOptions<BotConfiguration> options,
            IDbContextFactory<BotContext> dbContextFactory,
            TelegramBotClient telegramClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
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
                    
                    if (input.CfeVersionInfo != null)
                        ProcessCfeVersionInfo(input.CfeVersionInfo, cancellationToken);
                    
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
                    
                    if (input.BurnInfo != null)
                        ProcessBurnInfo(input.BurnInfo, cancellationToken);
                    
                    if (input.DailySwapOverviewInfo != null)
                        ProcessDailySwapOverviewInfo(input.DailySwapOverviewInfo, cancellationToken);
                    
                    if (input.WeeklySwapOverviewInfo != null)
                        ProcessWeeklySwapOverviewInfo(input.WeeklySwapOverviewInfo, cancellationToken);
                    
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
    }
}