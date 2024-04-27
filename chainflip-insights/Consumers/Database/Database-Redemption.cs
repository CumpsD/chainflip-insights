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
    using ChainflipInsights.Feeders.Redemption;
    using CsvHelper;
    using Microsoft.Extensions.Logging;

    public partial class DatabaseConsumer
    {
        private void ProcessRedemptionInfo(RedemptionInfo redemption)
        {
            if (redemption.AmountConverted < _configuration.DatabaseRedemptionAmountThreshold)
            {
                _logger.LogInformation(
                    "Redemption did not meet threshold (${Threshold}) for Database: {Validator} redeemed {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DatabaseRedemptionAmountThreshold,
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Storing Redemption in Database: {Validator} redeemed {Amount} FLIP -> {ExplorerUrl}",
                    redemption.Validator,
                    redemption.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));

                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    dbContext
                        .FundingInfo
                        .Add(new EntityFramework.FundingInfo
                        {
                            FundingDate = redemption.Timestamp,
                            Amount = -1 * redemption.Amount,
                            Epoch = redemption.Epoch,
                            Validator = redemption.Validator,
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Redemption {Validator} redeemed {Amount} FLIP -> {ExplorerUrl}",
                        redemption.Validator,
                        redemption.AmountFormatted,
                        string.Format(_configuration.ValidatorUrl, redemption.ValidatorName));
                    
                    GenerateRedemptionCsv(dbContext);
                }
                
                UploadRedemptionCsv(File.ReadAllText(_configuration.RedemptionCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
        
        private void GenerateRedemptionCsv(BotContext dbContext)
        {
            var fundingInfo = dbContext
                .FundingInfo
                .OrderBy(x => x.FundingDate)
                .Where(x => x.Amount < 0)
                .ToList();
            
            var funding = fundingInfo
                .Select(x =>
                {
                    var asset = Constants.SupportedAssets[Constants.FLIP];
                    
                    return new
                    {
                        FundingDate = x.FundingDate.Date.ToString("yyyy-MM-dd"),
                        Amount = (x.Amount / Math.Pow(10, asset.Decimals)).ToString(asset.FormatString),
                    };
                })
                .ToList();
     
            using var writer = new StreamWriter(_configuration.RedemptionCsvLocation, false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords((IEnumerable)funding.OrderBy(x => x.FundingDate).ToList());
            csv.Flush();
                    
            _logger.LogInformation(
                "Generated Redemption CSV {CsvPath}",
                _configuration.RedemptionCsvLocation);
        }
        
        private void UploadRedemptionCsv(string csv)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("DuneUpload");

                var csvUpload = new
                {
                    data = csv,
                    description = "Chainflip Redemption",
                    table_name = "flip_redemption",
                };

                _logger.LogInformation(
                    "Uploading Redemption CSV {CsvPath}",
                    _configuration.RedemptionCsvLocation);

                var result = client.PostAsJsonAsync(
                    $"{_configuration.DuneUploadUrl}?api_key={_configuration.DuneApiKey}",
                    csvUpload).GetAwaiter().GetResult();

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Uploaded Redemption CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    _logger.LogError(
                        "Uploaded Redemption CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Uploading Redemption CSV to Dune failed.");
            }
        }
    }
}