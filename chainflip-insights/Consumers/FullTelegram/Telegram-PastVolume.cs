namespace ChainflipInsights.Consumers.FullTelegram
{
    using System;
    using System.Linq;
    using System.Threading;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Infrastructure;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Requests;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class FullTelegramConsumer
    {
        private void ProcessPastVolumeInfo(
            PastVolumeInfo pastVolume,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordPastVolumeEnabled.Value)
            {
                _logger.LogInformation(
                    "Past Volume disabled for Full Telegram. {Date}: {Pairs} Past 24h Volume pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing Volume for {Date} on Full Telegram: {Pairs} Past 24h Volume Pairs.",
                    pastVolume.Date,
                    pastVolume.VolumePairs.Count);

                var totalVolume = pastVolume
                    .VolumePairs
                    .Sum(x => x.Value.Value);

                var totalFees = pastVolume
                    .VolumePairs
                    .Sum(x => x.Value.Fees);

                var burn = GetBurn(pastVolume.Date);
                
                var text =
                    $"📊 On **{pastVolume.Date}** we had a volume of " +
                    $"**${totalVolume.ToReadableMetric()}** with " +
                    $"**${pastVolume.NetworkFeesFormatted}** in network fees " +
                    $"and **${totalFees.ToReadableMetric()}** in liquidity provider fees.";

                if (!string.IsNullOrWhiteSpace(burn))
                    text += $" We also burned **{burn} FLIP**!";
                
                var message = _telegramClient
                    .SendMessageAsync(
                        new SendMessageRequest
                        {
                            ChatId = new ChatId(_configuration.TelegramInfoChannelId.Value),
                            Text = text,
                            ParseMode = ParseMode.Markdown,
                            DisableNotification = true,
                            LinkPreviewOptions = new LinkPreviewOptions
                            {
                                IsDisabled = true
                            },
                            ReplyParameters = new ReplyParameters
                            {
                                AllowSendingWithoutReply = true,
                            }
                        },
                        cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing Volume on Full Telegram as Message {MessageId}",
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
        
        private string? GetBurn(string date)
        {
            try
            {
                using var dbContext = _dbContextFactory.CreateDbContext();
                
                var burnDate = DateTimeOffset.Parse(date);

                var burn = dbContext.BurnInfo.SingleOrDefault(x => x.BurnDate.Date == burnDate.Date);

                return (burn?.BurnAmount / 1000000000000000000)?.ToString("###,###,###,###,##0.00");
            }
            catch
            {
                return null;
            }
        }
    }
}