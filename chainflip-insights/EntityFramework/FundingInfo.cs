namespace ChainflipInsights.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #pragma warning disable CS8618
    public class FundingInfo
    {
        public ulong FundingId { get; set; }
            
        public DateTimeOffset FundingDate { get; set; }

        public double Amount { get; set; }

        public ulong Epoch { get; set; }

        public string Validator { get; set; }
    }
    #pragma warning restore CS8618

    public class FundingInfoConfiguration : EntityTypeConfiguration<BotContext, FundingInfo>
    {
        private const string TableName = "funding_info";

        public override void Configure(EntityTypeBuilder<FundingInfo> builder)
        {
            builder
                .ToTable(TableName)
                .HasKey(x => x.FundingId);

            builder.Property(x => x.FundingDate).IsRequired();
            builder.HasIndex(x => x.FundingDate);
            
            builder.Property(x => x.Epoch).IsRequired();
            
            builder.Property(x => x.Amount).IsRequired();

            builder.Property(x => x.Validator).HasMaxLength(400).IsRequired();
        }
    }
}