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
        public bool? EnableCexMovement
        {
            get; init;
        }  
        
        [Required]
        [NotNull]
        public bool? EnableCfeVersion
        {
            get; init;
        }  
        
        [Required]
        [NotNull]
        public bool? EnableSwapLimits
        {
            get; init;
        }  
        
        [Required]
        [NotNull]
        public bool? EnablePastVolume
        {
            get; init;
        } 
        
        [Required]
        [NotNull]
        public bool? EnableStakedFlip
        {
            get; init;
        } 
        
        [Required]
        [NotNull]
        public bool? EnableBrokerOverview
        {
            get; init;
        } 
        
        [Required]
        [NotNull]
        public bool? EnableBigStakedFlip
        {
            get; init;
        } 
        
        [Required]
        [NotNull]
        public bool? EnableBurn
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
        public bool? EnableMastodon
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
        public string? MastodonClientKey
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? MastodonClientSecret
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? MastodonAccessToken
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? DuneApiKey
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? SubstrateEndpoint
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
        public string? UniswapV2Graph
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? UniswapV3Graph
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
        public string? ExplorerBlocksUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? ExplorerPoolsUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? EtherScanUrl
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
        public string? DuneUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? RpcUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? StakedFlipGraphUrl
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? DuneCexMovementQuery
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
        public int? CexMovementQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? CfeVersionQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? SwapLimitsQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? PastVolumeQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? StakedFlipQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? BrokerOverviewQueryDelay
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public int? BigStakedFlipQueryDelay
        {
            get; init;
        }        
        
        [Required]
        [NotNull]
        public int? BurnQueryDelay
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
        public string? LastCexMovementDayLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastCfeVersionLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastSwapLimitsLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastPastVolumeLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastStakedFlipLocation
        {
            get; init;
        }
                
        [Required]
        [NotNull]
        public string? LastBrokerOverviewLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastBigStakedFlipLocation
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastBurnLocation
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
        public long? TelegramInfoChannelId 
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? SwapWhitelist
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? MastodonSwapAmountThreshold
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
        public double? MastodonLiquidityAmountThreshold
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
        public double? MastodonFundingAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TwitterFundingAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TelegramFundingAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? DiscordFundingAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? MastodonRedemptionAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TwitterRedemptionAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TelegramRedemptionAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? DiscordRedemptionAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? MastodonEpochEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TwitterEpochEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TelegramEpochEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? DiscordEpochEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? MastodonCexMovementEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TwitterCexMovementEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TelegramCexMovementEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? DiscordCexMovementEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? MastodonCfeVersionEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TwitterCfeVersionEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TelegramCfeVersionEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? DiscordCfeVersionEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? MastodonSwapLimitsEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TwitterSwapLimitsEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TelegramSwapLimitsEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? DiscordSwapLimitsEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? MastodonPastVolumeEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TwitterPastVolumeEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TelegramPastVolumeEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? DiscordPastVolumeEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? MastodonStakedFlipEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TwitterStakedFlipEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TelegramStakedFlipEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? DiscordStakedFlipEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? MastodonBrokerOverviewEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TwitterBrokerOverviewEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TelegramBrokerOverviewEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? DiscordBrokerOverviewEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? MastodonStakedFlipAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TwitterStakedFlipAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? TelegramStakedFlipAmountThreshold
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public double? DiscordStakedFlipAmountThreshold
        {
            get; init;
        }
        
        
        [Required]
        [NotNull]
        public bool? MastodonBurnEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TwitterBurnEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? TelegramBurnEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public bool? DiscordBurnEnabled
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public Broker[]? Brokers
        {
            get; init;
        }
    }

    public class Broker
    {
        [Required]
        [NotNull]
        public string? Address
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? Name
        {
            get; init;
        }
        
        public string? Twitter
        {
            get; init;
        }
    }
}