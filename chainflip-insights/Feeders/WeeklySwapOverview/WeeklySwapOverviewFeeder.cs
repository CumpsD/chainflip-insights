namespace ChainflipInsights.Feeders.WeeklySwapOverview
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.EntityFramework;
    using ChainflipInsights.Infrastructure;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class WeeklySwapOverviewFeeder : IFeeder
    {
        private readonly ILogger<WeeklySwapOverviewFeeder> _logger;
        private readonly IDbContextFactory<BotContext> _dbContextFactory;
        private readonly Pipeline<WeeklySwapOverviewInfo> _pipeline;
        private readonly BotConfiguration _configuration;

        public WeeklySwapOverviewFeeder(
            ILogger<WeeklySwapOverviewFeeder> logger,
            IOptions<BotConfiguration> options,
            IDbContextFactory<BotContext> dbContextFactory,
            Pipeline<WeeklySwapOverviewInfo> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }
        
        public async Task Start()
        {
            try
            {
                if (!_configuration.EnableWeeklySwapOverview.Value)
                {
                    _logger.LogInformation(
                        "Weekly Swap Overview not enabled. Skipping {TaskName}",
                        nameof(WeeklySwapOverviewFeeder));

                    return;
                }
                
                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, _configuration.StartupDelay.Value), _pipeline.CancellationToken);

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(WeeklySwapOverviewFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching WeeklySwapOverview Info
                await ProvideWeeklySwapOverviewInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(WeeklySwapOverviewFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(WeeklySwapOverviewFeeder));
            }
        }

        private async Task ProvideWeeklySwapOverviewInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastWeeklySwapOverview = await GetLastWeeklySwapOverview(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                // Determine last week's dates
                var startingDate = DateTime.UtcNow.Date;

                while (startingDate.DayOfWeek != DayOfWeek.Monday)
                    startingDate = startingDate.AddDays(-1);

                var startOfWeek = startingDate.AddDays(-7);
                var endOfWeek = startingDate.AddDays(-1);
                
                var startOfWeekString = startOfWeek.ToString("yyyy-MM-dd");
                var endOfWeekString = endOfWeek.ToString("yyyy-MM-dd");
                
                var dateString = $"{startOfWeekString}|{endOfWeekString}";
                if (dateString == lastWeeklySwapOverview)
                {
                    _logger.LogInformation(
                        "No new weekly swap overview to announce. Last weekly swap info is still {WeeklySwapOverview}",
                        lastWeeklySwapOverview);
                    
                    await Task.Delay(_configuration.WeeklySwapOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                var weeklySwapOverviewInfo = await GetWeeklySwapOverview(
                    startOfWeek,
                    endOfWeek,
                    cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                if (weeklySwapOverviewInfo == null)
                {
                    await Task.Delay(_configuration.WeeklySwapOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var weeklySwapOverview = new WeeklySwapOverviewInfo(
                    startOfWeek,
                    endOfWeek,
                    weeklySwapOverviewInfo);
                
                _logger.LogInformation(
                    "Broadcasting weekly swap overview for {WeeklySwapOverview}",
                    dateString);

                await _pipeline.Source.SendAsync(
                    weeklySwapOverview, 
                    cancellationToken);
                
                lastWeeklySwapOverview = dateString;
                await StoreLastWeeklySwapOverview(lastWeeklySwapOverview);
                
                await Task.Delay(_configuration.WeeklySwapOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<string> GetLastWeeklySwapOverview(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastWeeklySwapOverviewLocation))
                return await File.ReadAllTextAsync(_configuration.LastWeeklySwapOverviewLocation, cancellationToken);

            await using var file = File.CreateText(_configuration.LastWeeklySwapOverviewLocation);
            await file.WriteAsync("1");
            return "1";
        }
        
        private async Task StoreLastWeeklySwapOverview(string lastWeeklySwapOverview)
        {
            await using var file = File.CreateText(_configuration.LastWeeklySwapOverviewLocation);
            await file.WriteAsync(lastWeeklySwapOverview.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<List<SwapOverviewAsset>?> GetWeeklySwapOverview(
            DateTimeOffset timeFrom,
            DateTimeOffset timeTo,
            CancellationToken cancellationToken)
        {
            /*
               SELECT *
               FROM Insights.swap_info
               WHERE SwapDate > '2024-08-22 00:00:00.000000'
               AND SwapDate < '2024-08-23 00:00:00.000000'
               AND SourceAsset = 'ETH' or DestinationAsset = 'ETH'
               ORDER BY EgressValueUsd DESC
               LIMIT 1
             */

            var assets = new[]
            {
                "BTC",
                "ETH",
                "DOT",
                "FLIP",
                "USDT",
                "arbETH",
                "arbUSDC"
            };
            
            try
            {
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

                var swapAsset = new List<SwapOverviewAsset>();

                foreach (var asset in assets)
                {
                    var swap = await dbContext
                        .SwapInfo
                        .Where(x =>
                            (x.SourceAsset == asset || x.DestinationAsset == asset) &&
                            x.SwapDate >= DateTimeOffset.Parse(timeFrom.ToString("yyyy-MM-ddT00:00:00.000Z")) &&
                            x.SwapDate < DateTimeOffset.Parse(timeTo.ToString("yyyy-MM-ddT00:00:00.000Z")))
                        .OrderByDescending(x => x.EgressValueUsd)
                        .Take(1)
                        .ToListAsync(cancellationToken);
                    
                    if (swap.Count == 1)
                        swapAsset.Add(new SwapOverviewAsset(asset, new SwapOverviewInfo(swap.First())));
                }

                return swapAsset;
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching weekly swap overview failed.");
            }

            return null;
        }
    }
}