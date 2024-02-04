namespace ChainflipInsights.Configuration
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    
    public class BotConfiguration
    {
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Convention for configuration is .Section")]
        public const string Section = "Bot";
        
        [Required]
        [NotNull]
        public bool? EnableSwaps
        {
            get; init;
        }  
        
        [Required]
        [NotNull]
        public bool? EnableLiquidity
        {
            get; init;
        }  
        
        [Required]
        [NotNull]
        public bool? EnableEpoch
        {
            get; init;
        }  
        
        [Required]
        [NotNull]
        public bool? EnableFunding
        {
            get; init;
        }  
                
        [Required]
        [NotNull]
        public bool? EnableRedemption
        {
            get; init;
        }  
        
        [Required]
        [NotNull]
        public bool? EnableDiscord
        {
            get; init;
        }  
        
        [Required]
        [NotNull]
        public bool? EnableTelegram
        {
            get; init;
        }   
        
        [Required]
        [NotNull]
        public bool? EnableTwitter
        {
            get; init;
        }   
        
        [Required]
        [NotNull]
        public string? DiscordToken
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? TelegramToken
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? GraphUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? ExplorerSwapsUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? ExplorerLiquidityChannelUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? ExplorerAuthorityUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? SwapUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? ValidatorUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? FeedingDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? EpochQueryDelay
        {
            get; init;
        }

        [Required]
        [NotNull]
        public int? IncomingLiquidityQueryDelay
        {
            get; init;
        }

        [Required]
        [NotNull]
        public int? SwapQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? FundingQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? RedemptionQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastSwapIdLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastIncomingLiquidityIdLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastEpochIdLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastFundingIdLocation
        {
            get; init;
        }

        [Required]
        [NotNull]
        public string? LastRedemptionIdLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public ulong? DiscordSwapInfoChannelId
        {
            get; init;
        }

        [Required]
        [NotNull]
        public long? TelegramSwapInfoChannelId 
        {
            get; init;
        }

        [Required]
        [NotNull]
        public string? TwitterConsumerKey
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? TwitterConsumerSecret
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? TwitterAccessToken
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? TwitterAccessTokenSecret
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TwitterSwapAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TelegramSwapAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? DiscordSwapAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TwitterLiquidityAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TelegramLiquidityAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? DiscordLiquidityAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? FundingAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? RedemptionAmountThreshold
        {
            get; init;
        }
    }
}