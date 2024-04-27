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
        private void ProcessIncomingLiquidityInfo(IncomingLiquidityInfo liquidity)
        {
            if (liquidity.DepositValueUsd < _configuration.DatabaseLiquidityAmountThreshold)
            {
                _logger.LogInformation(
                    "Incoming Liquidity did not meet threshold (${Threshold}) for Database: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    _configuration.DatabaseLiquidityAmountThreshold,
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Storing Incoming Liquidity in Database: {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                    liquidity.DepositAmountFormatted,
                    liquidity.SourceAsset,
                    liquidity.DepositValueUsdFormatted,
                    $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");

                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    dbContext
                        .LiquidityInfo
                        .Add(new EntityFramework.LiquidityInfo
                        {
                            LiquidityDate = liquidity.Timestamp,
                            Amount = liquidity.DepositAmount,
                            AmountUsd = liquidity.DepositValueUsd,
                            Asset = liquidity.SourceAsset,
                            ExplorerUrl = $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}"
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Incoming Liquidity {IngressAmount} {IngressTicker} (${IngressUsdAmount}) -> {ExplorerUrl}",
                        liquidity.DepositAmountFormatted,
                        liquidity.SourceAsset,
                        liquidity.DepositValueUsdFormatted,
                        $"{_configuration.ExplorerLiquidityChannelUrl}{liquidity.BlockId}-{liquidity.Network}-{liquidity.ChannelId}");
                    
                    GenerateIncomingLiquidityCsv(dbContext);
                }
                
                UploadIncomingLiquidityCsv(File.ReadAllText(_configuration.IncomingLiquidityCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
        
        private void GenerateIncomingLiquidityCsv(BotContext dbContext)
        {
            var liquidityInfo = dbContext
                .LiquidityInfo
                .OrderBy(x => x.LiquidityDate)
                .Where(x => x.Amount > 0)
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
     
            using var writer = new StreamWriter(_configuration.IncomingLiquidityCsvLocation, false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords((IEnumerable)funding.OrderBy(x => x.LiquidityDate).ToList());
            csv.Flush();
                    
            _logger.LogInformation(
                "Generated Incoming Liquidity CSV {CsvPath}",
                _configuration.IncomingLiquidityCsvLocation);
        }
        
        private void UploadIncomingLiquidityCsv(string csv)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("DuneUpload");

                var csvUpload = new
                {
                    data = csv,
                    description = "Chainflip Incoming Liquidity",
                    table_name = "flip_incoming_liquidity",
                };

                _logger.LogInformation(
                    "Uploading Incoming Liquidity CSV {CsvPath}",
                    _configuration.IncomingLiquidityCsvLocation);

                var result = client.PostAsJsonAsync(
                    $"{_configuration.DuneUploadUrl}?api_key={_configuration.DuneApiKey}",
                    csvUpload).GetAwaiter().GetResult();

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Uploaded Incoming Liquidity CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    _logger.LogError(
                        "Uploaded Incoming Liquidity CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Uploading Incoming Liquidity CSV to Dune failed.");
            }
        }
    }
}