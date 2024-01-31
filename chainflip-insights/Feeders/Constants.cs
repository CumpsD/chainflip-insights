namespace ChainflipInsights.Feeders
{
    using System.Collections.Generic;

    public static class Constants
    {
        public const string Sub1K = "🦐";
        public const string Sub2_5K = "🐟";
        public const string Sub5K = "🦀";
        public const string Sub10K = "🦈";
        public const string Whale = "🐳";
        
        public const string DollarString = "0.00";

        public static readonly Dictionary<string, int> AssetDecimals = new()
        {
            { "btc", 8 },
            { "dot", 10 },
            { "eth", 18 },
            { "flip", 18 },
            { "usdc", 6 },
        };
    }
}