namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Tweetinvi;

    public class TwitterConsumer
    {
        private readonly ILogger<TwitterConsumer> _logger;
        private readonly BotConfiguration _configuration;
        private readonly TwitterClient _twitterClient;

        public TwitterConsumer(
            ILogger<TwitterConsumer> logger,
            IOptions<BotConfiguration> options,
            TwitterClient twitterClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _twitterClient = twitterClient ?? throw new ArgumentNullException(nameof(twitterClient));
        }

        public ITargetBlock<SwapInfo> Build(
            CancellationToken ct)
        {
            var announcer = BuildAnnouncer(ct);
            return new EncapsulatingTarget<SwapInfo, SwapInfo>(announcer, announcer);
        }

        private ActionBlock<SwapInfo> BuildAnnouncer(
            CancellationToken ct)
        {
            var logging = new ActionBlock<SwapInfo>(
                swap =>
                {
                    if (!_configuration.EnableTwitter.Value)
                        return;

                    try
                    {
                        var text =
                            $"{swap.Emoji} Swapped {_configuration.ExplorerUrl}{swap.Id}\n" +
                            $"➡️ {swap.DepositAmountFormatted} ${swap.SourceAsset} (${swap.DepositValueUsdFormatted})\n" +
                            $"⬅️ {swap.EgressAmountFormatted} ${swap.DestinationAsset} (${swap.EgressValueUsdFormatted})";
                        
                        _twitterClient.Execute
                            .AdvanceRequestAsync(x =>
                            {
                                x.Query.Url = "https://api.twitter.com/2/tweets";
                                x.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                                x.Query.HttpContent = JsonContent.Create(
                                    new TweetV2PostRequest { Text = text },
                                    mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
                            })
                            .GetAwaiter()
                            .GetResult();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Twitter meh.");
                    }

                    Task
                        .Delay(1500, ct)
                        .GetAwaiter()
                        .GetResult();
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = ct
                });

            logging.Completion.ContinueWith(
                task => _logger.LogDebug(
                    "Twitter Logging completed, {Status}",
                    task.Status),
                ct);

            return logging;
        }
    }

    /// <summary>
    /// There are a lot more fields according to:
    /// https://developer.twitter.com/en/docs/twitter-api/tweets/manage-tweets/api-reference/post-tweets
    /// but these are the ones we care about for our use case.
    /// </summary>
    public class TweetV2PostRequest
    {
        /// <summary>
        /// The text of the tweet to post.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}