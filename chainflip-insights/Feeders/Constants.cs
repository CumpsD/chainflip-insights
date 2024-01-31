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

        public static readonly Dictionary<string, AssetInfo> AssetDecimals = new()
        {
            {
                "btc",
                new AssetInfo(
                    "btc",
                    "BTC",
                    "Bitcoin",
                    "Bitcoin",
                    8)
            },
            {
                "dot",
                new AssetInfo(
                    "dot",
                    "DOT",
                    "Polkadot",
                    "Polkadot",
                    10)
            },
            {
                "eth",
                new AssetInfo(
                    "eth",
                    "ETH",
                    "Ethereum",
                    "Ethereum",
                    18)
            },
            {
                "flip",
                new AssetInfo(
                    "flip",
                    "FLIP",
                    "Chainflip",
                    "Ethereum",
                    18)
            },
            {
                "usdc",
                new AssetInfo(
                    "usdc",
                    "USDC",
                    "ethUSDC",
                    "Ethereum",
                    6)
            },
        };
    }

    public class AssetInfo
    {
        public string Id { get; }

        public string Ticker { get; }
        
        public int Decimals { get; }
        
        public string FormatString { get; }

        public string Name { get; }

        public  string Network { get; }
        
        public AssetInfo(
            string id,
            string ticker, 
            string name,
            string network, 
            int decimals)
        {
            Id = id;
            Ticker = ticker;
            Name = name;
            Network = network;
            Decimals = decimals;
            
            FormatString = $"0.00{new string('#', decimals - 2)}";
        }
    }
}