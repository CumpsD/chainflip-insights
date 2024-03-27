namespace ChainflipInsights.Consumers.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure.Pipelines;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Tweetinvi;

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

    public partial class TwitterConsumer : IConsumer
    {
        private readonly ILogger<TwitterConsumer> _logger;
        private readonly BotConfiguration _configuration;
        private readonly TwitterClient _twitterClient;
        private readonly Dictionary<string, Broker> _brokers;

        public TwitterConsumer(
            ILogger<TwitterConsumer> logger,
            IOptions<BotConfiguration> options,
            TwitterClient twitterClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _twitterClient = twitterClient ?? throw new ArgumentNullException(nameof(twitterClient));
            
            _brokers = _configuration
                .Brokers
                .ToDictionary(
                    x => x.Address,
                    x => x);
        }

        public ITargetBlock<BroadcastInfo> Build(
            CancellationToken cancellationToken)
        {
            var announcer = BuildAnnouncer(cancellationToken);
            return new EncapsulatingTarget<BroadcastInfo, BroadcastInfo>(announcer, announcer);
        }

        private ActionBlock<BroadcastInfo> BuildAnnouncer(
            CancellationToken cancellationToken)
        {
            var logging = new ActionBlock<BroadcastInfo>(
                input =>
                {
                    if (!_configuration.EnableTwitter.Value)
                        return;

                    if (input.SwapInfo != null)
                        ProcessSwap(input.SwapInfo);
                    
                    if (input.IncomingLiquidityInfo != null)
                        ProcessIncomingLiquidityInfo(input.IncomingLiquidityInfo);
                    
                    if (input.EpochInfo != null)
                        ProcessEpochInfo(input.EpochInfo);
                    
                    if (input.FundingInfo != null)
                        ProcessFundingInfo(input.FundingInfo);
                    
                    if (input.RedemptionInfo != null)
                        ProcessRedemptionInfo(input.RedemptionInfo);
                    
                    if (input.CexMovementInfo != null)
                        ProcessCexMovementInfo(input.CexMovementInfo);
                    
                    if (input.SwapLimitsInfo != null)
                        ProcessSwapLimitsInfo(input.SwapLimitsInfo);
                    
                    if (input.PastVolumeInfo != null)
                        ProcessPastVolumeInfo(input.PastVolumeInfo);
                    
                    if (input.StakedFlipInfo != null)
                        ProcessStakedFlipInfo(input.StakedFlipInfo);
                    
                    if (input.BrokerOverviewInfo != null)
                        ProcessBrokerOverviewInfo(input.BrokerOverviewInfo);
                    
                    if (input.BigStakedFlipInfo != null)
                        ProcessBigStakedFlipInfo(input.BigStakedFlipInfo);
                    
                    if (input.BurnInfo != null)
                        ProcessBurnInfo(input.BurnInfo);
                    
                    Task
                        .Delay(1500, cancellationToken)
                        .GetAwaiter()
                        .GetResult();
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = cancellationToken
                });

            logging.Completion.ContinueWith(
                task => _logger.LogInformation(
                    "Twitter Logging completed, {Status}",
                    task.Status),
                cancellationToken);

            return logging;
        }
    }
}