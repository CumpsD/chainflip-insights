namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using ChainflipInsights.Feeders.Funding;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessFundingInfo(FundingInfo funding)
        {
            if (funding.AmountConverted < _configuration.MastodonFundingAmountThreshold)
            {
                _logger.LogInformation(
                    "Funding did not meet threshold (${Threshold}) for Mastodon: {Validator} added {Amount} FLIP -> {ExplorerUrl}",
                    _configuration.MastodonFundingAmountThreshold,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Mastodon: {Validator} added {Amount} FLIP -> {EpochUrl}",
                    funding.Id,
                    funding.Validator,
                    funding.AmountFormatted,
                    string.Format(_configuration.ValidatorUrl, funding.ValidatorName));

                var text =
                    $"🪙 Validator {funding.Validator} added {funding.AmountFormatted} #FLIP! {string.Format(_configuration.ValidatorUrl, funding.ValidatorName)}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Funding {FundingId} on Mastodon as Message {MessageId}",
                    funding.Id,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}