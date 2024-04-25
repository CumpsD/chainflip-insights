namespace ChainflipInsights.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #pragma warning disable CS8618
    public class LiquidityInfo
    {
        public ulong LiquidityId { get; set; }
            
        public DateTimeOffset LiquidityDate { get; set; }

        public double Amount { get; set; }

        public double AmountUsd { get; set; }

        public string Asset { get; set; }

        public string ExplorerUrl { get; set; }
    }
    #pragma warning restore CS8618

    public class LiquidityInfoConfiguration : EntityTypeConfiguration<BotContext, LiquidityInfo>
    {
        private const string TableName = "liquidity_info";

        public override void Configure(EntityTypeBuilder<LiquidityInfo> builder)
        {
            builder
                .ToTable(TableName)
                .HasKey(x => x.LiquidityId);

            builder.Property(x => x.LiquidityDate).IsRequired();
            builder.HasIndex(x => x.LiquidityDate);
            
            builder.Property(x => x.Amount).IsRequired();
            
            builder.Property(x => x.AmountUsd).IsRequired();
            
            builder.Property(x => x.Asset).HasMaxLength(10).IsRequired();
            builder.HasIndex(x => x.Asset);
            
            builder.Property(x => x.ExplorerUrl).IsRequired();
        }
    }
}