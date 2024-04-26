namespace ChainflipInsights.Consumers.Database
{
    using System;
    using ChainflipInsights.Feeders.Funding;
    using Microsoft.Extensions.Logging;

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
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Database meh.");
            }
        }
    }
}