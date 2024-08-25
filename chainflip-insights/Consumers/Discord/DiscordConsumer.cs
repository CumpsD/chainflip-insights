namespace ChainflipInsights.Consumers.Discord
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
    using global::Discord;
    using global::Discord.WebSocket;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public partial class DiscordConsumer : IConsumer
    {
        private readonly ILogger<DiscordConsumer> _logger;
        private readonly IDbContextFactory<BotContext> _dbContextFactory;
        private readonly BotConfiguration _configuration;
        private readonly DiscordSocketClient _discordClient;
        private readonly Dictionary<string, string> _brokers;
        private readonly Dictionary<string, string> _liquidityProviders;

        private readonly Emoji _tadaEmoji = new("🎉");
        private readonly Emoji _angryEmoji = new("🤬");

        public DiscordConsumer(
            ILogger<DiscordConsumer> logger,
            IOptions<BotConfiguration> options,
            IDbContextFactory<BotContext> dbContextFactory,
            DiscordSocketClient discordClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));

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
                    
                    if (input.PastVolumeInfo != null)
                        ProcessPastVolumeInfo(input.PastVolumeInfo);
                    
                    if (input.StakedFlipInfo != null)
                        ProcessStakedFlipInfo(input.StakedFlipInfo);
                    
                    if (input.BrokerOverviewInfo != null)
                        ProcessBrokerOverviewInfo(input.BrokerOverviewInfo);
                    
                    if (input.BigStakedFlipInfo != null)
                        ProcessBigStakedFlipInfo(input.BigStakedFlipInfo);
                    
                    if (input.BurnInfo != null)
                        ProcessBurnInfo(input.BurnInfo);
                    
                    if (input.DailySwapOverviewInfo != null)
                        ProcessDailySwapOverviewInfo(input.DailySwapOverviewInfo);
                    
                    if (input.WeeklySwapOverviewInfo != null)
                        ProcessWeeklySwapOverviewInfo(input.WeeklySwapOverviewInfo);
                    
                    if (input.DailyLpOverviewInfo != null)
                        ProcessDailyLpOverviewInfo(input.DailyLpOverviewInfo);
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
