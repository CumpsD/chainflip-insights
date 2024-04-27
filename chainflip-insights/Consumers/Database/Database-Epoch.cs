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
    using CsvHelper;
    using Microsoft.Extensions.Logging;
    using EpochInfo = ChainflipInsights.Feeders.Epoch.EpochInfo;

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
                            TotalBond = epoch.TotalBond,
                            TotalRewards = epoch.PreviousEpoch!.TotalRewards,
                            ExplorerUrl = $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}"
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Epoch {Epoch} in Database -> {EpochUrl}",
                        epoch.Id,
                        $"{_configuration.ExplorerAuthorityUrl}{epoch.Id}");
                    
                    GenerateEpochCsv(dbContext);
                }
                
                UploadEpochCsv(File.ReadAllText(_configuration.EpochCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
        
        private void GenerateEpochCsv(BotContext dbContext)
        {
            var epochInfo = dbContext
                .EpochInfo
                .OrderBy(x => x.EpochId)
                .ToList();
            
            var epochs = epochInfo
                .Select(x =>
                {
                    var asset = Constants.SupportedAssets[Constants.FLIP];
                    var decimals = Math.Pow(10, asset.Decimals);
                    
                    return new
                    {
                        Epoch = x.EpochId,
                        EpochDate = x.EpochStart.Date.ToString("yyyy-MM-dd"),
                        MinimumBond = (x.MinimumBond / decimals).ToString(asset.FormatString), 
                        TotalBond = (x.TotalBond / decimals).ToString(asset.FormatString), 
                        MaxBid = (x.MaxBid / decimals).ToString(asset.FormatString), 
                        TotalRewards = (x.TotalRewards / decimals).ToString(asset.FormatString), 
                    };
                })
                .ToList();
     
            using var writer = new StreamWriter(_configuration.EpochCsvLocation, false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords((IEnumerable)epochs.OrderBy(x => x.Epoch).ToList());
            csv.Flush();
                    
            _logger.LogInformation(
                "Generated Epoch CSV {CsvPath}",
                _configuration.EpochCsvLocation);
        }
        
        private void UploadEpochCsv(string csv)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("DuneUpload");

                var csvUpload = new
                {
                    data = csv,
                    description = "Chainflip Epochs",
                    table_name = "flip_epochs",
                };

                _logger.LogInformation(
                    "Uploading Epoch CSV {CsvPath}",
                    _configuration.EpochCsvLocation);

                var result = client.PostAsJsonAsync(
                    $"{_configuration.DuneUploadUrl}?api_key={_configuration.DuneApiKey}",
                    csvUpload).GetAwaiter().GetResult();

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Uploaded Epoch CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    _logger.LogError(
                        "Uploaded Epoch CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Uploading Epoch CSV to Dune failed.");
            }
        }
    }
}