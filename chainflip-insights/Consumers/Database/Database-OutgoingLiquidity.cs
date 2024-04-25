namespace ChainflipInsights.Consumers.Database
{
    using System;
    using ChainflipInsights.Feeders.Liquidity;
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
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
    }
}