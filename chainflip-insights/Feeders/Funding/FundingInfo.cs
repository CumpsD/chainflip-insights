namespace ChainflipInsights.Feeders.Funding
{
    using System;

    public class FundingInfo
    {
        private const int FlipDecimals = 18;
        
        public double Id { get; }
        
        public double Amount { get; }
        
        public string AmountFormatted => Math.Round(Amount / Math.Pow(10, FlipDecimals), 3).ToString("0.00");
        
        public double Epoch { get; }
        
        public string ValidatorAlias { get; set; }

        public string ValidatorName { get; set; }
        
        public DateTimeOffset Timestamp { get; }


        public FundingInfo(
            FundingInfoResponseNode funding)
        {
            Id = funding.Id;
            Amount = funding.Amount;
            Epoch = funding.Epoch;
            ValidatorAlias = funding.Validator.Alias;
            ValidatorName = funding.Validator.Name;
            Timestamp = funding.Event.Block.Timestamp;
        }
    }
}