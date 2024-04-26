namespace ChainflipInsights.Consumers.Database
{
    using System;
    using ChainflipInsights.Feeders.Epoch;
    using Microsoft.Extensions.Logging;

    public partial class DatabaseConsumer
    {
        private void ProcessEpochInfo(EpochInfo epoch)
        {
            if (!_configuration.DatabaseEpochEnabled.Value)
            {
                _logger.LogInformation(
                    "Epoch disabled for Database. Epoch {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Storing Epoch in Database: {Epoch} -> {EpochUrl}",
                    epoch.Id,
                    $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");

                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    dbContext
                        .EpochInfo
                        .Add(new EntityFramework.EpochInfo
                        {
                            EpochId = Convert.ToUInt64(epoch.Id),
                            EpochStart = epoch.EpochStart,
                            MinimumBond = epoch.MinimumBond,
                            MaxBid = epoch.MaxBid,
                            TotalRewards = epoch.TotalRewards,
                            ExplorerUrl = $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}"
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Epoch {Epoch} in Database -> {EpochUrl}",
                        epoch.Id,
                        $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
    }
}