namespace ChainflipInsights.Feeders.DailySwapOverview
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

    public class DailySwapOverviewFeeder : IFeeder
    {
        private readonly ILogger<DailySwapOverviewFeeder> _logger;
        private readonly IDbContextFactory<BotContext> _dbContextFactory;
        private readonly Pipeline<DailySwapOverviewInfo> _pipeline;
        private readonly BotConfiguration _configuration;

        public DailySwapOverviewFeeder(
            ILogger<DailySwapOverviewFeeder> logger,
            IOptions<BotConfiguration> options,
            IDbContextFactory<BotContext> dbContextFactory,
            Pipeline<DailySwapOverviewInfo> pipeline)
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
                if (!_configuration.EnableDailySwapOverview.Value)
                {
                    _logger.LogInformation(
                        "Daily Swap Overview not enabled. Skipping {TaskName}",
                        nameof(DailySwapOverviewFeeder));

                    return;
                }
                
                // Add some randomization before starting to not spam the world
                await Task.Delay(Random.Shared.Next(0, _configuration.StartupDelay.Value), _pipeline.CancellationToken);

                _logger.LogInformation(
                    "Starting {TaskName}",
                    nameof(DailySwapOverviewFeeder));

                // Give the consumers some time to connect
                await Task.Delay(_configuration.FeedingDelay.Value, _pipeline.CancellationToken);

                // Start a loop fetching DailySwapOverview Info
                await ProvideDailySwapOverviewInfo(_pipeline.CancellationToken);

                _logger.LogInformation(
                    "Stopping {TaskName}",
                    nameof(DailySwapOverviewFeeder));
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Something went wrong in {TaskName}",
                    nameof(DailySwapOverviewFeeder));
            }
        }

        private async Task ProvideDailySwapOverviewInfo(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            var lastDailySwapOverview = await GetLastDailySwapOverview(cancellationToken);
            
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var yesterday = DateTime.UtcNow.Date.AddDays(-1);
                var dateString = yesterday.ToString("yyyy-MM-dd");
                if (dateString == lastDailySwapOverview)
                {
                    _logger.LogInformation(
                        "No new daily swap overview to announce. Last daily swap info is still {DailySwapOverview}",
                        lastDailySwapOverview);
                    
                    await Task.Delay(_configuration.DailySwapOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;
                }
                
                var dailySwapOverviewInfo = await GetDailySwapOverview(
                    yesterday,
                    yesterday.AddDays(1),
                    cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                    return;
                
                if (dailySwapOverviewInfo == null)
                {
                    await Task.Delay(_configuration.DailySwapOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
                    continue;                    
                }
                
                var dailySwapOverview = new DailySwapOverviewInfo(
                    yesterday, 
                    dailySwapOverviewInfo.Select(x => new SwapOverviewInfo(x)));
                
                _logger.LogInformation(
                    "Broadcasting daily swap overview for {DailySwapOverview}",
                    dateString);

                await _pipeline.Source.SendAsync(
                    dailySwapOverview, 
                    cancellationToken);
                
                lastDailySwapOverview = dateString;
                await StoreLastDailySwapOverview(lastDailySwapOverview);
                
                await Task.Delay(_configuration.DailySwapOverviewQueryDelay.Value.RandomizeTime(), cancellationToken);
            }
        }
        
        private async Task<string> GetLastDailySwapOverview(CancellationToken cancellationToken)
        {
            if (File.Exists(_configuration.LastDailySwapOverviewLocation))
                return await File.ReadAllTextAsync(_configuration.LastDailySwapOverviewLocation, cancellationToken);

            await using var file = File.CreateText(_configuration.LastDailySwapOverviewLocation);
            await file.WriteAsync("2024-08-21");
            return "2024-08-21";
        }
        
        private async Task StoreLastDailySwapOverview(string lastDailySwapOverview)
        {
            await using var file = File.CreateText(_configuration.LastDailySwapOverviewLocation);
            await file.WriteAsync(lastDailySwapOverview.ToString(CultureInfo.InvariantCulture));
        }
        
        private async Task<List<SwapInfo>?> GetDailySwapOverview(
            DateTimeOffset timeFrom,
            DateTimeOffset timeTo,
            CancellationToken cancellationToken)
        {
            /*
               SELECT *
               FROM Insights.swap_info
               WHERE SwapDate > '2024-08-22 00:00:00.000000'
               AND SwapDate < '2024-08-23 00:00:00.000000'
               ORDER BY EgressValueUsd DESC
               LIMIT 5 
             */
            
            try
            {
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                
                return await dbContext
                    .SwapInfo
                    .Where(x =>
                        x.SwapDate >= DateTimeOffset.Parse(timeFrom.ToString("yyyy-MM-ddT00:00:00.000Z")) &&
                        x.SwapDate < DateTimeOffset.Parse(timeTo.ToString("yyyy-MM-ddT00:00:00.000Z")))
                    .OrderByDescending(x => x.EgressValueUsd)
                    .Take(5)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Fetching daily swap overview failed.");
            }

            return null;
        }
    }
}