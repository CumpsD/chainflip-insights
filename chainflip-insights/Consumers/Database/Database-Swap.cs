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
    using SwapInfo = ChainflipInsights.Feeders.Swap.SwapInfo;

    public partial class DatabaseConsumer
    {
        private void ProcessSwap(SwapInfo swap)
        {
            if (swap.DepositValueUsd < _configuration.DatabaseSwapAmountThreshold &&
                !_configuration.SwapWhitelist.Contains(swap.DestinationAsset, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogInformation(
                    "Swap did not meet threshold (${Threshold}) for Database: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker} -> {ExplorerUrl}",
                    _configuration.DiscordSwapAmountThreshold,
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                return;
            }

            try
            {
                var brokerExists = _brokers.TryGetValue(swap.Broker ?? string.Empty, out var broker);
                
                _logger.LogInformation(
                    "Storing Swap in Database: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker}{Broker} -> {ExplorerUrl}",
                    swap.DepositAmountFormatted,
                    swap.SourceAsset,
                    swap.EgressAmountFormatted,
                    swap.DestinationAsset,
                    brokerExists ? $" @ {broker}" : string.Empty,
                    $"{_configuration.ExplorerSwapsUrl}{swap.Id}");

                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    dbContext
                        .SwapInfo
                        .Add(new EntityFramework.SwapInfo
                        {
                            SwapId = Convert.ToUInt64(swap.Id),
                            SwapDate = swap.SwapScheduledBlockTimestamp,
                            DepositAmount = swap.DepositAmount,
                            DepositValueUsd = swap.DepositValueUsd,
                            SourceAsset = swap.SourceAsset,
                            EgressAmount = swap.EgressAmount,
                            EgressValueUsd = swap.EgressValueUsd,
                            DestinationAsset = swap.DestinationAsset,
                            DeltaUsd = swap.DeltaUsd,
                            DeltaUsdPercentage = swap.DeltaUsdPercentage,
                            Broker = swap.Broker,
                            ExplorerUrl = $"{_configuration.ExplorerSwapsUrl}{swap.Id}"
                        });

                    dbContext.SaveChanges();

                    _logger.LogInformation(
                        "Stored Swap: {IngressAmount} {IngressTicker} to {EgressAmount} {EgressTicker}{Broker} -> {ExplorerUrl}",
                        swap.DepositAmountFormatted,
                        swap.SourceAsset,
                        swap.EgressAmountFormatted,
                        swap.DestinationAsset,
                        brokerExists ? $" @ {broker}" : string.Empty,
                        $"{_configuration.ExplorerSwapsUrl}{swap.Id}");
                    
                    GenerateSwapCsv(dbContext);
                }
                
                UploadSwapCsv(File.ReadAllText(_configuration.SwapCsvLocation));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
        
        private void GenerateSwapCsv(BotContext dbContext)
        {
            var swapInfo = dbContext
                .SwapInfo
                .OrderBy(x => x.SwapId)
                .ToList();
            
            var swaps = swapInfo
                .Select(x =>
                {
                    // TODO: This will need to take chain into account
                    var sourceAsset = Constants.SupportedAssets[x.SourceAsset.ToLower()];
                    var destinationAsset = Constants.SupportedAssets[x.DestinationAsset.ToLower()];
                    
                    return new
                    {
                        Id = x.SwapId,
                        SwapDate = x.SwapDate.Date.ToString("yyyy-MM-dd"),
                        DepositAmount = (x.DepositAmount / Math.Pow(10, sourceAsset.Decimals)).ToString(sourceAsset.FormatString),
                        DepositAmountUsd = x.DepositValueUsd.ToString(Constants.DollarString),
                        DepositAsset = x.SourceAsset,
                        DestinationAmount = (x.EgressAmount / Math.Pow(10, destinationAsset.Decimals)).ToString(destinationAsset.FormatString),
                        DestinationAmountUsd = x.EgressValueUsd.ToString(Constants.DollarString),
                        DestinationAsset = x.DestinationAsset,
                        DeltaUsd = x.DeltaUsd,
                        DeltaUsdPercentage = x.DeltaUsdPercentage
                    };
                })
                .ToList();
        
            using var writer = new StreamWriter(_configuration.SwapCsvLocation, false);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords((IEnumerable)swaps.OrderBy(x => x.Id).ToList());
            csv.Flush();
                    
            _logger.LogInformation(
                "Generated Swap CSV {CsvPath}",
                _configuration.SwapCsvLocation);
        }
        
        private void UploadSwapCsv(string csv)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("DuneUpload");
        
                var csvUpload = new
                {
                    data = csv,
                    description = "Chainflip Swaps",
                    table_name = "flip_swaps",
                };
        
                _logger.LogInformation(
                    "Uploading Swap CSV {CsvPath}",
                    _configuration.SwapCsvLocation);
        
                var result = client.PostAsJsonAsync(
                    $"{_configuration.DuneUploadUrl}?api_key={_configuration.DuneApiKey}",
                    csvUpload).GetAwaiter().GetResult();
        
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Uploaded Swap CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
                else
                {
                    _logger.LogError(
                        "Uploaded Swap CSV {Result} -> {Response}",
                        result.StatusCode,
                        result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Uploading Swap CSV to Dune failed.");
            }
        }
    }
}