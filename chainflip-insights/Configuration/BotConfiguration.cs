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
        public string? Token
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
        public ulong? SwapInfoChannelId
        {
            get; init;
        }
        
        [Required]
        [NotNull]
        public string? LastSwapIdLocation
        {
            get; init;
        }
    }
}