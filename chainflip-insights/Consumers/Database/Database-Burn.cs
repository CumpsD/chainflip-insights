namespace ChainflipInsights.Consumers.Database
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using ChainflipInsights.Feeders.Burn;
    using CsvHelper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public partial class DatabaseConsumer
    {
        private void ProcessBurnInfo(BurnInfo burn)
        {
            if (!_configuration.DatabaseBurnEnabled.Value)
            {
                _logger.LogInformation(
                    "Burn disabled for Database. Burn {BurnBlock} ({BurnBlockHash}) -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                return;
            }
            
            if (burn.BurnSkipped)
            {
                _logger.LogInformation(
                    "Burn did not meet burn threshold. Burn {BurnBlock} ({BurnBlockHash}) -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Storing Burn {BurnBlock} ({BurnBlockHash}) in Database -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                using var dbContext = _dbContextFactory.CreateDbContext();
                
                dbContext
                    .BurnInfo
                    .Add(new EntityFramework.BurnInfo
                    {
                        BurnDate = DateTimeOffset.UtcNow,
                        BurnBlock = burn.LastSupplyUpdateBlock,
                        BurnHash = burn.LastSupplyUpdateBlockHash,
                        BurnAmount = burn.FlipBurned!.Value,
                        ExplorerUrl = $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}"
                    });

                dbContext.SaveChanges();
                
                _logger.LogInformation(
                    "Stored Burn {BurnBlock} ({BurnBlockHash}) in Database -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    burn.LastSupplyUpdateBlockHash,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");
                
                // TODO: Generate CSV and upload to dune
                var burnInfo = dbContext
                    .BurnInfo
                    .OrderBy(x => x.BurnDate)
                    .ToList()
                    .Select(x => new
                    {
                        x.BurnDate,
                        x.BurnAmount
                    });

                using var writer = new StreamWriter(_configuration.BurnCsvLocation, false);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords((IEnumerable)burnInfo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
    }
}