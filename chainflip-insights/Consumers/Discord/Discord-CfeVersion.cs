namespace ChainflipInsights.Consumers.Discord
{
    using System;
    using System.Linq;
    using ChainflipInsights.Feeders.CfeVersion;
    using global::Discord;
    using Microsoft.Extensions.Logging;

    public partial class DiscordConsumer
    {
        private void ProcessCfeVersionInfo(CfeVersionsInfo cfeVersionInfo)
        {
            if (!_configuration.DiscordCfeVersionEnabled.Value)
            {
                _logger.LogInformation(
                    "CFE Version disabled for Discord. {Date}: {Versions} CFE Versions.",
                    cfeVersionInfo.Date,
                    cfeVersionInfo.Versions.Count);

                return;
            }

            if (_discordClient.ConnectionState != ConnectionState.Connected)
                return;

            try
            {
                _logger.LogInformation(
                    "Announcing CFE Versions for {Date} on Discord: {Versions} CFE Versions.",
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

                var infoChannel = (ITextChannel)_discordClient
                    .GetChannel(_configuration.DiscordSwapInfoChannelId.Value);

                var message = infoChannel
                    .SendMessageAsync(
                        text,
                        flags: MessageFlags.SuppressEmbeds)
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation(
                    "Announcing CFE Versions {Day} on Discord as Message {MessageId}",
                    cfeVersionInfo.Date,
                    message.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Discord meh.");
            }
        }
    }
}