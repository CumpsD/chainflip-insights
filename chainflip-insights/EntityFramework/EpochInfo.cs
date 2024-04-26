namespace ChainflipInsights.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #pragma warning disable CS8618
    public class EpochInfo
    {
        public ulong EpochId { get; set; }
        
        public DateTimeOffset EpochStart { get; set; }
            
        public double MinimumBond { get; set; }

        public double TotalBond { get; set; }

        public double MaxBid { get; set; }

        public double TotalRewards { get; set; }

        public string ExplorerUrl { get; set; }
    }
    #pragma warning restore CS8618

    public class EpochInfoConfiguration : EntityTypeConfiguration<BotContext, EpochInfo>
    {
        private const string TableName = "epoch_info";

        public override void Configure(EntityTypeBuilder<EpochInfo> builder)
        {
            builder
                .ToTable(TableName)
                .HasKey(x => x.EpochId);

            builder.Property(x => x.EpochStart).IsRequired();
            builder.HasIndex(x => x.EpochStart);
            
            builder.Property(x => x.MinimumBond).IsRequired();
            
            builder.Property(x => x.TotalBond).IsRequired();
            
            builder.Property(x => x.MaxBid).IsRequired();
            
            builder.Property(x => x.TotalRewards).IsRequired();

            builder.Property(x => x.ExplorerUrl).IsRequired();
        }
    }
}