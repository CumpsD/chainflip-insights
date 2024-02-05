namespace ChainflipInsights.Feeders.CexMovement
{
    using System;

    public class CexMovementInfo
    {
        public int DayOfYear { get; set; }
        
        public double TotalMovement { get; set; }
        
        public double MovementIn { get; set; }
        
        public double MovementOut { get; set; }
        
        public CexMovementInfo(
            CexMovementInfoResponseRow cexMovement)
        {
            DayOfYear = cexMovement.DayOfYear;
            TotalMovement = cexMovement.TotalMovement;
            MovementIn = cexMovement.MovementIn;
            MovementOut = cexMovement.MovementOut;
        }
    }
}