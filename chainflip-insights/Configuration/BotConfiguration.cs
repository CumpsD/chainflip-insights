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
        public string? ExplorerUrl
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
        public int? SwapInfoDelay
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
    }
}