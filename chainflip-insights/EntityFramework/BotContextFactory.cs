namespace ChainflipInsights.EntityFramework
{
    using System;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class BotContextFactory : IDesignTimeDbContextFactory<BotContext>
    {
        public BotContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            EntityFrameworkLogger.LoggerFactory = new LoggerFactory();
            
            var optionsBuilder = new DbContextOptionsBuilder<BotContext>();
            var connectionString = configuration.GetConnectionString("Migrations");
            
            optionsBuilder.UseMySql(connectionString, Db.Version);
            
            return new BotContext(optionsBuilder.Options);
        }
    }
}