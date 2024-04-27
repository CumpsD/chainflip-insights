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
    using FundingInfo = ChainflipInsights.Feeders.Funding.FundingInfo;

    public partial class DatabaseConsumer
    {
        private void ProcessFundingInfo(FundingInfo funding)
        {
            if (funding.AmountConverted < _configuration.DatabaseFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet threshold (${Threshold}) for Database: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.DatabaseFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Storing Funding in Database: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));

                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    dbContext
                        .FundingInfo
                        .Add(new EntityFramework.FundingInfo
                        {
                            FundingDate = funding.Timestamp,
                            Amount = funding.Amount,
                            Epoch = funding.Epoch,
                            Validator = funding.Validator,
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Funding {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                        funding.Validator,
                        funding.AmountFormatted,
                        string.Format(_configuration.ValidatorUrl, funding.ValidatorName));
                    
                    GenerateFundingCsv(dbContext);
                }
                
                UploadFundingCsv(File.ReadAllText(_configuration.FundingCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
        
        private void GenerateFundingCsv(BotContext dbContext)
        {
            var fundingInfo = dbContext
                .FundingInfo
                .OrderBy(x => x.FundingDate)
                .Where(x => x.Amount > 0)
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
     
            using var writer = new StreamWriter(_configuration.FundingCsvLocation, false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords((IEnumerable)funding.OrderBy(x => x.FundingDate).ToList());
            csv.Flush();
                    
            _logger.LogInformation(
                "Generated Funding CSV {CsvPath}",
                _configuration.FundingCsvLocation);
        }
        
        private void UploadFundingCsv(string csv)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("DuneUpload");

                var csvUpload = new
                {
                    data = csv,
                    description = "Chainflip Funding",
                    table_name = "flip_funding",
                };

                _logger.LogInformation(
                    "Uploading Funding CSV {CsvPath}",
                    _configuration.FundingCsvLocation);

                var result = client.PostAsJsonAsync(
                    $"{_configuration.DuneUploadUrl}?api_key={_configuration.DuneApiKey}",
                    csvUpload).GetAwaiter().GetResult();

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Uploaded Funding CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    _logger.LogError(
                        "Uploaded Funding CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Uploading Funding CSV to Dune failed.");
            }
        }
    }
}