namespace ChainflipInsights.Feeders.Redemption
{
    using System;

    public class RedemptionInfo
    {
        private const int FlipDecimals = 18;
        
        public double Id { get; }
        
        public double Amount { get; }
        
        public double AmountConverted => Amount / Math.Pow(10, FlipDecimals);
        
        public string AmountFormatted => Math.Round(Amount / Math.Pow(10, FlipDecimals), 3).ToString("0.00");
        
        public double Epoch { get; }
        
        public string Validator { get; }
        
        public string ValidatorAlias { get; }

        public string ValidatorName { get; }
        
        public DateTimeOffset Timestamp { get; }
        
        public RedemptionInfo(
            RedemptionInfoResponseNode redemption)
        {
            Id = redemption.Id;
            Amount = redemption.Amount;
            Epoch = redemption.Epoch;
            ValidatorAlias = redemption.Validator.Alias;
            ValidatorName = redemption.Validator.Name;
            Timestamp = redemption.Event.Block.Timestamp;
            
            Validator =
                string.IsNullOrWhiteSpace(ValidatorAlias)
                    ? ValidatorName
                    : $"{ValidatorName} ({ValidatorAlias})";
        }
    }
}