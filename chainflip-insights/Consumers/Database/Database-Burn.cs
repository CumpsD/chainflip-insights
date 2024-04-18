namespace ChainflipInsights.Consumers.Database
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using ChainflipInsights.Feeders.Burn;
    using ChainflipInsights.Infrastructure;
    using CsvHelper;
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
                
                var burnInfo = dbContext
                    .BurnInfo
                    .OrderBy(x => x.BurnDate)
                    .ToList();
                
                var burnDates = burnInfo.Select(x => x.BurnDate.Date);
                var startDate = burnInfo.Min(x => x.BurnDate).Date;
                var endDate = burnInfo.Max(x => x.BurnDate).Date;
                var missing = startDate.Range(endDate).Except(burnDates);
                
                var burns = burnInfo
                    .Select(x => new
                    {
                        BurnDate = x.BurnDate.Date.ToString("yyyy-MM-dd"),
                        BurnAmount = (x.BurnAmount / 1000000000000000000).ToString("###,###,###,###,##0.000000000000000000")
                    })
                    .ToList();

                burns.AddRange(missing.Select(x => new
                {
                    BurnDate = x.ToString("yyyy-MM-dd"),
                    BurnAmount = "0.000000000000000000"
                }));
                
                using var writer = new StreamWriter(_configuration.BurnCsvLocation, false);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords((IEnumerable)burns.OrderBy(x => x.BurnDate).ToList());
                csv.Flush();
                
                _logger.LogInformation(
                    "Generated Burn CSV {CsvPath}",
                    _configuration.BurnCsvLocation);

                UploadCsv(File.ReadAllText(_configuration.BurnCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
        
        private void UploadCsv(string csv)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("DuneUpload");

                var csvUpload = new
                {
                    data = csv,
                    description = "Chainflip Burns",
                    table_name = "flip_burns",
                };

                _logger.LogInformation(
                    "Uploading Burn CSV {CsvPath}",
                    _configuration.BurnCsvLocation);

                var result = client.PostAsJsonAsync(
                    $"{_configuration.DuneUploadUrl}?api_key={_configuration.DuneApiKey}",
                    csvUpload).GetAwaiter().GetResult();

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Uploaded Burn CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    _logger.LogError(
                        "Uploaded Burn CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Uploading CSV to Dune failed.");
            }
        }
    }
}