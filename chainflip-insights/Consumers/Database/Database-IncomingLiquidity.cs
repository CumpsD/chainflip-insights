namespace ChainflipInsights.Consumers.Database
{
    using System;
    using ChainflipInsights.Feeders.Liquidity;
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
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
    }
}