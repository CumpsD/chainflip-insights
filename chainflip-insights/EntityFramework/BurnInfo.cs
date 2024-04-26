namespace ChainflipInsights.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #pragma warning disable CS8618
    public class BurnInfo
    {
        public ulong BurnId { get; set; }
            
        public DateTimeOffset BurnDate { get; set; }

        public ulong BurnBlock { get; set; }

        public string BurnHash { get; set; }

        public double BurnAmount { get; set; }

        public string ExplorerUrl { get; set; }
    }
    #pragma warning restore CS8618

    public class BurnInfoConfiguration : EntityTypeConfiguration<BotContext, BurnInfo>
    {
        private const string TableName = "burn_info";

        public override void Configure(EntityTypeBuilder<BurnInfo> builder)
        {
            builder
                .ToTable(TableName)
                .HasKey(x => x.BurnId);

            builder.Property(x => x.BurnDate).IsRequired();
            builder.HasIndex(x => x.BurnDate);
            
            builder.Property(x => x.BurnBlock).IsRequired();
            
            builder.Property(x => x.BurnHash).HasMaxLength(120).IsRequired();
            
            builder.Property(x => x.BurnAmount).IsRequired();

            builder.Property(x => x.ExplorerUrl).IsRequired();
        }
    }
}