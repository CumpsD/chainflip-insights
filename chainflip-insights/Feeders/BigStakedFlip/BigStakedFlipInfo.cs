namespace ChainflipInsights.Feeders.BigStakedFlip
{
    using System;
    using ChainflipInsights.Infrastructure;

    public class BigStakedFlipInfo
    {
        public DateTimeOffset Date { get; }
        public double Staked { get; }

        public string StakedFormatted  => Staked.ToReadableMetric();

        public string To { get; }

        public string TransactionHash { get; }

        public BigStakedFlipInfo(
            BigStakedFlipData staked)
        {
            var flip = Constants.SupportedAssets[Constants.FLIP];

            Staked = Math.Round(staked.Amount / Math.Pow(10, flip.Decimals));

            To = staked.To;
            TransactionHash = staked.TransactionHash;
            Date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(staked.BlockTimestamp));
        }
    }
}