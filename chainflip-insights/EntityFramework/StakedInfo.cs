namespace ChainflipInsights.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #pragma warning disable CS8618
    public class StakedInfo
    {
        public ulong StakedId { get; set; }
        
        public DateTimeOffset Date { get; set; }
            
        public double TotalMovement { get; set; }

        public double Staked { get; set; }

        public double Unstaked { get; set; }

        public string NetMovement { get; set; }
    }
    #pragma warning restore CS8618

    public class StakedInfoConfiguration : EntityTypeConfiguration<BotContext, StakedInfo>
    {
        private const string TableName = "staked_info";

        public override void Configure(EntityTypeBuilder<StakedInfo> builder)
        {
            builder
                .ToTable(TableName)
                .HasKey(x => x.StakedId);

            builder.Property(x => x.Date).IsRequired();
            builder.HasIndex(x => x.Date);
            
            builder.Property(x => x.TotalMovement).IsRequired();
            
            builder.Property(x => x.Staked).IsRequired();
            
            builder.Property(x => x.Unstaked).IsRequired();

            builder.Property(x => x.NetMovement).HasMaxLength(15).IsRequired();
        }
    }
}