namespace ChainflipInsights.Feeders.CexMovement
{
    using System;
    using ChainflipInsights.Infrastructure;
    
    public enum NetMovement
    {
        MoreTowardsCex,
        MoreTowardsDex
    }

    public class CexMovementInfo
    {
        public int DayOfYear { get; }
        
        public double TotalMovement { get; }
        
        public string TotalMovementFormatted => TotalMovement.ToReadableMetric();
        
        public NetMovement NetMovement { get; }
        
        public double MovementIn { get; }
        
        public string MovementInFormatted => MovementIn.ToReadableMetric();
        
        public double MovementOut { get; }
        
        public string MovementOutFormatted => MovementOut.ToReadableMetric();
        
        public DateTimeOffset Date { get; }
        
        public CexMovementInfo(
            CexMovementInfoResponseRow cexMovement)
        {
            DayOfYear = cexMovement.DayOfYear;
            TotalMovement = Math.Abs(cexMovement.TotalMovement);
            MovementIn = cexMovement.MovementIn;
            MovementOut = Math.Abs(cexMovement.MovementOut);

            NetMovement = cexMovement.TotalMovement > 0 
                ? NetMovement.MoreTowardsCex 
                : NetMovement.MoreTowardsDex;
            
            Date = new DateTime(DateTime.UtcNow.Year, 1, 1).AddDays(cexMovement.DayOfYear -1);
        }
    }
}