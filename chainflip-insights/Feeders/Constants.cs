namespace ChainflipInsights.Feeders
{
    using System.Collections.Generic;

    public static class Constants
    {
        public const string Shrimp = "🦐";
        public const string Fish = "🐟";
        public const string Crab = "🦀";
        public const string Shark = "🦈";
        public const string Whale = "🐳";
        
        public const string DollarString = "###,###,###,###,##0.00";

        public const string BTC = "btc";
        public const string DOT = "dot";
        public const string ETH = "eth";
        public const string FLIP = "flip";
        public const string USDC = "usdc";
        public const string USDT = "usdt";
        
        public static readonly Dictionary<string, AssetInfo> SupportedAssets = new()
        {
            {
                BTC,
                new AssetInfo(
                    BTC,
                    "BTC",
                    "Bitcoin",
                    "Bitcoin",
                    8)
            },
            {
                DOT,
                new AssetInfo(
                    DOT,
                    "DOT",
                    "Polkadot",
                    "Polkadot",
                    10)
            },
            {
                ETH,
                new AssetInfo(
                    ETH,
                    "ETH",
                    "Ethereum",
                    "Ethereum",
                    18)
            },
            {
                FLIP,
                new AssetInfo(
                    FLIP,
                    "FLIP",
                    "Chainflip",
                    "Ethereum",
                    18)
            },
            {
                USDC,
                new AssetInfo(
                    USDC,
                    "USDC",
                    "ethUSDC",
                    "Ethereum",
                    6)
            },
            {
                USDT,
                new AssetInfo(
                    USDT,
                    "USDT",
                    "ethUSDT",
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
            
            FormatString = $"###,###,###,###,##0.00{new string('#', decimals - 2)}";
        }
    }
}