namespace ChainflipInsights.Consumers.Database
{
    using System;
    using ChainflipInsights.Feeders.Redemption;
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
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
    }
}