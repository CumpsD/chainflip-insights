namespace ChainflipInsights.Consumers.Mastodon
{
    using System;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using Mastonet;
    using Microsoft.Extensions.Logging;

    public partial class MastodonConsumer
    {
        private void ProcessBigStakedFlipInfo(BigStakedFlipInfo bigStakedFlipInfo)
        {
            if (bigStakedFlipInfo.Staked < _configuration.MastodonStakedFlipAmountThreshold)
            {
                _logger.LogInformation(
                    "Staked flip did not meet threshold ({Threshold} FLIP) for Mastodon: {Amount} FLIP",
                    _configuration.MastodonStakedFlipAmountThreshold,
                    bigStakedFlipInfo.StakedFormatted);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing staked flip on Mastodon: {Amount} FLIP -> {ExplorerUrl}",
                    bigStakedFlipInfo.StakedFormatted,
                    $"{_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}");

                var text =
                    $"🔥 Big #stFLIP Alert! " +
                    $"{bigStakedFlipInfo.StakedFormatted} #FLIP just got staked! " +
                    $"// {_configuration.EtherScanUrl}{bigStakedFlipInfo.TransactionHash}\n" +
                    $"#chainflip #flip";

                var status = _mastodonClient
                    .PublishStatus(
                        text,
                        Visibility.Public)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing staked flip {TransactionHash} on Mastodon as Message {MessageId}",
                    bigStakedFlipInfo.TransactionHash,
                    status.Url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mastodon meh.");
            }
        }
    }
}