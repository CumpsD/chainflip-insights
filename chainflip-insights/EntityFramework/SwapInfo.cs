namespace ChainflipInsights.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #pragma warning disable CS8618
    public class SwapInfo
    {
        public ulong SwapId { get; set; }
            
        public DateTimeOffset SwapDate { get; set; }

        public double DepositAmount { get; set; }

        public double DepositValueUsd { get; set; }

        public string SourceAsset { get; set; }
        
        public double EgressAmount { get; set; }

        public double EgressValueUsd { get; set; }

        public string DestinationAsset { get; set; }
        
        public double DeltaUsd { get; set; }

        public double DeltaUsdPercentage { get; set; }
        
        public string? Broker { get; set; }

        public string ExplorerUrl { get; set; }
        
        public double? BoostFeeBps { get; set; }
        
        public double? BoostFeeUsd { get; set; }
        
        public double? BrokerFeeUsd { get; set; }
        
        public double? AllFeesUsd { get; set; }
        
        public double? AllUserFeesUsd { get; set; }
        
        public double? LiquidityFeesUsd { get; set; }
        
        public double? BurnUsd { get; set; }

    }
    #pragma warning restore CS8618

    public class SwapInfoConfiguration : EntityTypeConfiguration<BotContext, SwapInfo>
    {
        private const string TableName = "swap_info";

        public override void Configure(EntityTypeBuilder<SwapInfo> builder)
        {
            builder
                .ToTable(TableName)
                .HasKey(x => x.SwapId);

            builder.Property(x => x.SwapDate).IsRequired();
            builder.HasIndex(x => x.SwapDate);
            
            builder.Property(x => x.DepositAmount).IsRequired();
            builder.Property(x => x.DepositValueUsd).IsRequired();
            builder.Property(x => x.SourceAsset).HasMaxLength(10).IsRequired();

            builder.Property(x => x.EgressAmount).IsRequired();
            builder.Property(x => x.EgressValueUsd).IsRequired();
            builder.Property(x => x.DestinationAsset).HasMaxLength(10).IsRequired();
            
            builder.Property(x => x.DeltaUsd).IsRequired();
            builder.Property(x => x.DeltaUsdPercentage).IsRequired();
            
            builder.Property(x => x.Broker).HasMaxLength(400).IsRequired(false);

            builder.Property(x => x.ExplorerUrl).IsRequired();
            
            builder.Property(x => x.BoostFeeBps).IsRequired(false);
            builder.Property(x => x.BoostFeeUsd).IsRequired(false);
            builder.Property(x => x.BrokerFeeUsd).IsRequired(false);
            
            builder.Property(x => x.AllFeesUsd).IsRequired(false);
            builder.Property(x => x.AllUserFeesUsd).IsRequired(false);
            builder.Property(x => x.LiquidityFeesUsd).IsRequired(false);
            builder.Property(x => x.BurnUsd).IsRequired(false);
        }
    }
}