namespace ChainflipInsights.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    public class BotContext : DbContext
    {
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
        public virtual DbSet<BurnInfo> BurnInfo { get; set; } = null!;
        public virtual DbSet<LiquidityInfo> LiquidityInfo { get; set; } = null!;
        public virtual DbSet<FundingInfo> FundingInfo { get; set; } = null!;
        public virtual DbSet<EpochInfo> EpochInfo { get; set; } = null!;
        public virtual DbSet<StakedInfo> StakedInfo { get; set; } = null!;
        public virtual DbSet<SwapInfo> SwapInfo { get; set; } = null!;
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
        
        public BotContext() { }

        // This needs to be DbContextOptions<T> for Autofac!
        public BotContext(
            DbContextOptions<BotContext> options)
            : base(options) { }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddEntityConfigurationsFromAssembly<BotContext>(typeof(BotContext).Assembly);
        }
    }
}