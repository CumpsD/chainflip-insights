namespace ChainflipInsights.Consumers.Database
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Json;
    using ChainflipInsights.EntityFramework;
    using ChainflipInsights.Feeders;
    using ChainflipInsights.Infrastructure;
    using CsvHelper;
    using Microsoft.Extensions.Logging;
    using BurnInfo = ChainflipInsights.Feeders.Burn.BurnInfo;

    public partial class DatabaseConsumer
    {
        private void ProcessBurnInfo(BurnInfo burn)
        {
            if (!_configuration.DatabaseBurnEnabled.Value)
            {
                _logger.LogInformation(
                    "Burn disabled for Database. Burn {BurnBlock} -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                return;
            }
            
            try
            {
                _logger.LogInformation(
                    "Storing Burn {BurnBlock} in Database -> {BlockUrl}",
                    burn.LastSupplyUpdateBlock,
                    $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    dbContext
                        .BurnInfo
                        .Add(new EntityFramework.BurnInfo
                        {
                            BurnDate = burn.LastBurnTime,
                            BurnBlock = burn.LastSupplyUpdateBlock,
                            BurnAmount = burn.FlipBurned!.Value,
                            ExplorerUrl = $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}"
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Burn {BurnBlock} in Database -> {BlockUrl}",
                        burn.LastSupplyUpdateBlock,
                        $"{_configuration.ExplorerBlocksUrl}{burn.LastSupplyUpdateBlock}");

                    GenerateBurnCsv(dbContext);
                }

                UploadBurnCsv(File.ReadAllText(_configuration.BurnCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }

        private void GenerateBurnCsv(BotContext dbContext)
        {
            var burnInfo = dbContext
                .BurnInfo
                .OrderBy(x => x.BurnDate)
                .ToList();

            var burnDates = burnInfo.Select(x => x.BurnDate.Date);
            var startDate = burnInfo.Min(x => x.BurnDate).Date;
            var endDate = burnInfo.Max(x => x.BurnDate).Date;
            var missing = startDate.Range(endDate).Except(burnDates);

            var burns = burnInfo
                .Select(x =>
                {
                    var asset = Constants.SupportedAssets[Constants.FLIP];
                    
                    return new
                    {
                        BurnDate = x.BurnDate.Date.ToString("yyyy-MM-dd"),
                        BurnAmount = (x.BurnAmount / Math.Pow(10, asset.Decimals)).ToString(asset.FormatString),
                    };
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
        }

        private void UploadBurnCsv(string csv)
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
                    "Uploading Burn CSV to Dune failed.");
            }
        }
    }
}