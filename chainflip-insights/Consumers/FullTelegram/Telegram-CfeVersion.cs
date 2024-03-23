namespace ChainflipInsights.Consumers.FullTelegram
{
    using System;
    using System.Linq;
    using System.Threading;
    using ChainflipInsights.Feeders.CfeVersion;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Logging;

    public partial class FullTelegramConsumer
    {
        private void ProcessCfeVersionInfo(
            CfeVersionsInfo cfeVersionInfo,
            CancellationToken cancellationToken)
        {
            if (!_configuration.DiscordCfeVersionEnabled.Value)
            {
                _logger.LogInformation(
                    "CFE Version disabled for Full Telegram. {Date}: {Versions} CFE Versions.",
                    cfeVersionInfo.Date,
                    cfeVersionInfo.Versions.Count);

                return;
            }

            try
            {
                _logger.LogInformation(
                    "Announcing CFE Versions for {Date} on Full Telegram: {Versions} CFE Versions.",
                    cfeVersionInfo.Date,
                    cfeVersionInfo.Versions.Count);

                var maxVersion = cfeVersionInfo.Versions.Keys.Max(x => x);
                var upToDateValidators = cfeVersionInfo
                    .Versions[maxVersion]
                    .Validators
                    .Count(x => x.ValidatorStatus == ValidatorStatus.Online);

                var outdatedValidators = cfeVersionInfo
                    .Versions
                    .Where(x => 
                        x.Key < maxVersion &&
                        x.Value.Validators.Any(v => v.ValidatorStatus == ValidatorStatus.Online))
                    .ToList();

                var outdatedSum = outdatedValidators
                    .Sum(x => x.Value.Validators.Count(v => v.ValidatorStatus == ValidatorStatus.Online));

                var text =
                    $"📜 CFE overview for **{cfeVersionInfo.Date}**! " +
                    $"The current version is **{maxVersion}**, which **{upToDateValidators} online validators** are running. " +
                    (outdatedSum == 1
                        ? $"There is **{outdatedSum} online validator** on an older version: "
                        : $"There are **{outdatedSum} online validators** on older versions{(outdatedSum != 0 ? ": " : ".")}") +
                    $"{string.Join(", ", outdatedValidators.Select(x => $"**{x.Value.Validators.Count(v => v.ValidatorStatus == ValidatorStatus.Online)}** on **{x.Key}**"))}";

                var message = _telegramClient
                    .SendTextMessageAsync(
                        new ChatId(_configuration.TelegramInfoChannelId.Value),
                        text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        allowSendingWithoutReply: true,
                        cancellationToken: cancellationToken)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing CFE Versions {Day} on Full Telegram as Message {MessageId}",
                    cfeVersionInfo.Date,
                    message.MessageId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Full Telegram meh.");
            }
        }
    }
}