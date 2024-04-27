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
    using ChainflipInsights.Feeders.Liquidity;
    using CsvHelper;
    using Microsoft.Extensions.Logging;

    public partial class DatabaseConsumer
    {
        private void ProcessOutgoingLiquidityInfo(OutgoingLiquidityInfo liquidity)
        {
            if (liquidity.WithdrawalValueUsd < _configuration.DatabaseLiquidityAmountThreshold)
            {
                _logger.LogInformation(
                    "Outgoing Liquidity did not meet threshold (${Threshold}) for Database: {EgressAmount} {EgressTicker} (${EgressUsdAmount}) -> {ExplorerUrl}",
                    _configuration.DatabaseLiquidityAmountThreshold,
                    liquidity.WithdrawalAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.WithdrawalValueUsd,
                    $"{_configuration.ExplorerBlocksUrl}{liquidity.BlockId}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Storing Outgoing Liquidity in Database: {EgressAmount} {EgressTicker} (${EgressUsdAmount}) -> {ExplorerUrl}",
                    liquidity.WithdrawalAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.WithdrawalValueUsd,
                    $"{_configuration.ExplorerBlocksUrl}{liquidity.BlockId}");

                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    dbContext
                        .LiquidityInfo
                        .Add(new EntityFramework.LiquidityInfo
                        {
                            LiquidityDate = liquidity.Timestamp,
                            Amount = -1 * liquidity.WithdrawalAmount,
                            AmountUsd = -1 * liquidity.WithdrawalValueUsd,
                            Asset = liquidity.SourceAsset,
                            ExplorerUrl = $"{_configuration.ExplorerBlocksUrl}{liquidity.BlockId}"
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Outgoing Liquidity {EgressAmount} {EgressTicker} (${EgressUsdAmount}) -> {ExplorerUrl}",
                        liquidity.WithdrawalAmountFormatted,
                        liquidity.SourceAsset,
                        liquidity.WithdrawalValueUsd,
                        $"{_configuration.ExplorerBlocksUrl}{liquidity.BlockId}");
                    
                    GenerateOutgoingLiquidityCsv(dbContext);
                }
                
                UploadOutgoingLiquidityCsv(File.ReadAllText(_configuration.OutgoingLiquidityCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
        
        private void GenerateOutgoingLiquidityCsv(BotContext dbContext)
        {
            var liquidityInfo = dbContext
                .LiquidityInfo
                .OrderBy(x => x.LiquidityDate)
                .Where(x => x.Amount < 0)
                .ToList();
            
            var funding = liquidityInfo
                .Select(x =>
                {
                    var asset = Constants.SupportedAssets[x.Asset.ToLower()];
                    
                    return new
                    {
                        LiquidityDate = x.LiquidityDate.Date.ToString("yyyy-MM-dd"),
                        Amount = (x.Amount / Math.Pow(10, asset.Decimals)).ToString(asset.FormatString),
                        AmountUsd = x.AmountUsd,
                        Asset = x.Asset
                    };
                })
                .ToList();
     
            using var writer = new StreamWriter(_configuration.OutgoingLiquidityCsvLocation, false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords((IEnumerable)funding.OrderBy(x => x.LiquidityDate).ToList());
            csv.Flush();
                    
            _logger.LogInformation(
                "Generated Outgoing Liquidity CSV {CsvPath}",
                _configuration.OutgoingLiquidityCsvLocation);
        }
        
        private void UploadOutgoingLiquidityCsv(string csv)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("DuneUpload");

                var csvUpload = new
                {
                    data = csv,
                    description = "Chainflip Outgoing Liquidity",
                    table_name = "flip_outgoing_liquidity",
                };

                _logger.LogInformation(
                    "Uploading Incoming Liquidity CSV {CsvPath}",
                    _configuration.OutgoingLiquidityCsvLocation);

                var result = client.PostAsJsonAsync(
                    $"{_configuration.DuneUploadUrl}?api_key={_configuration.DuneApiKey}",
                    csvUpload).GetAwaiter().GetResult();

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Uploaded Outgoing Liquidity CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    _logger.LogError(
                        "Uploaded Outgoing Liquidity CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Uploading Outgoing Liquidity CSV to Dune failed.");
            }
        }
    }
}