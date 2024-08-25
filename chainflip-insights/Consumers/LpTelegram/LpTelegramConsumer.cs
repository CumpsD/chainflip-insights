namespace ChainflipInsights.Consumers.LpTelegram
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

    public partial class LpTelegramConsumer : IConsumer
    {
        private readonly ILogger<LpTelegramConsumer> _logger;
        private readonly BotConfiguration _configuration;
        private readonly TelegramBotClient _telegramClient;
        private readonly Dictionary<string,string> _brokers;
        private readonly Dictionary<string, string> _liquidityProviders;

        public LpTelegramConsumer(
            ILogger<LpTelegramConsumer> logger,
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
            
            _liquidityProviders = _configuration
                .LiquidityProviders
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
                    
                    if (input.DailyLpOverviewInfo != null)
                        ProcessDailyLpOverviewInfo(input.DailyLpOverviewInfo, cancellationToken);
                    
                    if (input.WeeklyLpOverviewInfo != null)
                        ProcessWeeklyLpOverviewInfo(input.WeeklyLpOverviewInfo, cancellationToken);
                    
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