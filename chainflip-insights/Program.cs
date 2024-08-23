namespace ChainflipInsights
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Discord;
    using Discord.WebSocket;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Consumers.Database;
    using ChainflipInsights.Consumers.Discord;
    using ChainflipInsights.Consumers.FullTelegram;
    using ChainflipInsights.Consumers.LpTelegram;
    using ChainflipInsights.Consumers.Mastodon;
    using ChainflipInsights.Consumers.Telegram;
    using ChainflipInsights.Consumers.Twitter;
    using ChainflipInsights.EntityFramework;
    using ChainflipInsights.Feeders;
    using ChainflipInsights.Feeders.BigStakedFlip;
    using ChainflipInsights.Feeders.BrokerOverview;
    using ChainflipInsights.Feeders.Burn;
    using ChainflipInsights.Feeders.CexMovement;
    using ChainflipInsights.Feeders.CfeVersion;
    using ChainflipInsights.Feeders.DailySwapOverview;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.PastVolume;
    using ChainflipInsights.Feeders.StakedFlip;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Feeders.SwapLimits;
    using ChainflipInsights.Infrastructure;
    using ChainflipInsights.Infrastructure.Options;
    using ChainflipInsights.Infrastructure.Pipelines;
    using ChainflipInsights.Modules;
    using Mastonet;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Telegram.Bot;
    using Tweetinvi;
    using BurnInfo = ChainflipInsights.Feeders.Burn.BurnInfo;
    using EpochInfo = ChainflipInsights.Feeders.Epoch.EpochInfo;
    using FundingInfo = ChainflipInsights.Feeders.Funding.FundingInfo;
    using SwapInfo = ChainflipInsights.Feeders.Swap.SwapInfo;

    public class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new();
        
        public static void Main()
        {
            var ct = CancellationTokenSource.Token;

            AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
                Log.Fatal(
                    (Exception)eventArgs.ExceptionObject,
                    "Encountered a fatal exception, exiting program.");
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();
            
            var container = ConfigureServices(configuration, ct);
            var logger = container.GetRequiredService<ILogger<Program>>();
            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name;
            var efLogger = container.GetRequiredService<EntityFrameworkLogger>();
            
            logger.LogInformation(
                "Starting {ApplicationName}",
                applicationName);

            var pipelines = new IPipeline[]
            {
                container.GetRequiredService<Pipeline<SwapInfo>>(),
                container.GetRequiredService<Pipeline<IncomingLiquidityInfo>>(),
                container.GetRequiredService<Pipeline<OutgoingLiquidityInfo>>(),
                container.GetRequiredService<Pipeline<EpochInfo>>(),
                container.GetRequiredService<Pipeline<FundingInfo>>(),
                container.GetRequiredService<Pipeline<RedemptionInfo>>(),
                container.GetRequiredService<Pipeline<CexMovementInfo>>(),
                container.GetRequiredService<Pipeline<CfeVersionsInfo>>(),
                container.GetRequiredService<Pipeline<SwapLimitsInfo>>(),
                container.GetRequiredService<Pipeline<PastVolumeInfo>>(),
                container.GetRequiredService<Pipeline<StakedFlipInfo>>(),
                container.GetRequiredService<Pipeline<BrokerOverviewInfo>>(),
                container.GetRequiredService<Pipeline<BigStakedFlipInfo>>(),
                container.GetRequiredService<Pipeline<BurnInfo>>(),
                container.GetRequiredService<Pipeline<DailySwapOverviewInfo>>(),
            };
            
            var feeders = new IFeeder[]
            {
                container.GetRequiredService<SwapFeeder>(),
                container.GetRequiredService<IncomingLiquidityFeeder>(),
                container.GetRequiredService<OutgoingLiquidityFeeder>(),
                container.GetRequiredService<EpochFeeder>(),
                container.GetRequiredService<FundingFeeder>(),
                container.GetRequiredService<RedemptionFeeder>(),
                container.GetRequiredService<CexMovementFeeder>(),
                container.GetRequiredService<CfeVersionFeeder>(),
                container.GetRequiredService<SwapLimitsFeeder>(),
                container.GetRequiredService<PastVolumeFeeder>(),
                container.GetRequiredService<StakedFlipFeeder>(),
                container.GetRequiredService<BrokerOverviewFeeder>(),
                container.GetRequiredService<BigStakedFlipFeeder>(),
                container.GetRequiredService<BurnFeeder>(),
                container.GetRequiredService<DailySwapOverviewFeeder>(),
            };
            
            Console.CancelKeyPress += (_, eventArgs) =>
            { 
                logger.LogInformation("Requesting stop...");

                foreach (var pipeline in pipelines)
                    pipeline?.SourceBlock.Complete();                

                CancellationTokenSource.Cancel();

                eventArgs.Cancel = true;
            };

            try
            {
                #if DEBUG
                Console.WriteLine($"Press ENTER to start {applicationName}...");
                Console.ReadLine();
                #endif
                
                var runner = container.GetRequiredService<Runner>();
                
                var tasks = new List<Task>();
                tasks.AddRange(runner.Start());
                tasks.AddRange(feeders.Select(feeder => feeder.Start()));

                Console.WriteLine("Running... Press CTRL + C to exit.");
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception e)
            {
                var tasksCancelled = false;
                if (e is AggregateException ae)
                    tasksCancelled = ae.InnerExceptions.All(x => x is TaskCanceledException);

                if (!tasksCancelled)
                {
                    logger.LogCritical(e, "Encountered a fatal exception, exiting program.");
                    throw;
                }
            }
            
            logger.LogInformation("Stopping...");
            
            // Allow some time for flushing before shutdown.
            Log.CloseAndFlush();
            Thread.Sleep(1000);
        }

        private static AutofacServiceProvider ConfigureServices(
            IConfiguration configuration,
            CancellationToken ct)
        {
            var services = new ServiceCollection();

            var builder = new ContainerBuilder();

            var connectionString = configuration.GetConnectionString("Bot");
            
            builder
                .RegisterModule(new LoggingModule(configuration, services));

            var botConfiguration = configuration
                .GetSection(BotConfiguration.Section)
                .Get<BotConfiguration>()!;
            
            services
                .ConfigureAndValidate<BotConfiguration>(configuration.GetSection(BotConfiguration.Section))
                                    
                .AddDbContextFactory<BotContext>((provider, options) => options
                    .UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>())
                    .UseMySql(connectionString, Db.Version))
                
                .AddSingleton(x => new EntityFrameworkLogger(x.GetRequiredService<ILoggerFactory>()))
                
                .AddHttpClient(
                    "Graph",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.GraphUrl);
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    })
                
                .Services
                    
                .AddHttpClient(
                    "StakedFlipGraph",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.StakedFlipGraphUrl.Replace("API_KEY", botConfiguration.TheGraphApiKey));
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    })
                
                .Services
                
                .AddHttpClient(
                    "Swap",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.SwapUrl);
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    })
                
                .Services
                
                .AddHttpClient(
                    "Dune",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.DuneUrl);
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    })
                
                .Services
                
                .AddHttpClient(
                    "DuneUpload",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.DuneUploadUrl);
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    })
                
                .Services
                
                .AddHttpClient(
                    "Rpc",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.RpcUrl);
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    })
                
                .Services
                    
                .AddHttpClient(
                    "UniswapV2Graph",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.UniswapV2Graph.Replace("API_KEY", botConfiguration.TheGraphApiKey));
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    })
                
                .Services
                    
                .AddHttpClient(
                    "UniswapV3Graph",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.UniswapV3Graph.Replace("API_KEY", botConfiguration.TheGraphApiKey));
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    });
            
            builder
                .Register(x =>
                {
                    var client = new DiscordSocketClient(new DiscordSocketConfig
                    {
                        GatewayIntents =
                            GatewayIntents.Guilds |
                            GatewayIntents.GuildMessages,
                        LogGatewayIntentWarnings = true,
                        AlwaysDownloadUsers = false,
                        LogLevel = LogSeverity.Debug
                    });

                    var logger = x.Resolve<ILogger<Program>>();
                    
                    client.Log += message =>
                    {
                        message.Log(logger);
                        return Task.CompletedTask;
                    };
                    
                    client.Ready += () =>
                    {
                        logger.LogInformation("Discord Client Ready!");
                        return Task.CompletedTask;
                    };
                    
                    client.Disconnected += exception =>
                    {
                        if (exception is GatewayReconnectException)
                        {
                            logger.LogInformation(
                                exception, 
                                $"Reconnecting: {exception.Message}");

                            return Task.CompletedTask;
                        }
                
                        logger.LogError(
                            exception, 
                            exception.Message);
                
                        Log.CloseAndFlush();
                
                        Environment.Exit(-1);
                
                        return Task.CompletedTask;
                    }; 
            
                    return client;
                })
                .SingleInstance();

            builder
                .Register(_ => new TelegramBotClient(botConfiguration.TelegramToken))
                .SingleInstance();

            builder
                .Register(_ => new TwitterClient(
                    botConfiguration.TwitterConsumerKey,
                    botConfiguration.TwitterConsumerSecret,
                    botConfiguration.TwitterAccessToken,
                    botConfiguration.TwitterAccessTokenSecret))
                .SingleInstance();

            builder
                .Register(_ => new MastodonClient("mastodon.social", botConfiguration.MastodonAccessToken))
                .SingleInstance();
            
            RegisterFeeder<SwapFeeder, SwapInfo>(builder, ct);
            RegisterFeeder<IncomingLiquidityFeeder, IncomingLiquidityInfo>(builder, ct);
            RegisterFeeder<OutgoingLiquidityFeeder, OutgoingLiquidityInfo>(builder, ct);
            RegisterFeeder<EpochFeeder, EpochInfo>(builder, ct);
            RegisterFeeder<FundingFeeder, FundingInfo>(builder, ct);
            RegisterFeeder<RedemptionFeeder, RedemptionInfo>(builder, ct);
            RegisterFeeder<CexMovementFeeder, CexMovementInfo>(builder, ct);
            RegisterFeeder<CfeVersionFeeder, CfeVersionsInfo>(builder, ct);
            RegisterFeeder<SwapLimitsFeeder, SwapLimitsInfo>(builder, ct);
            RegisterFeeder<PastVolumeFeeder, PastVolumeInfo>(builder, ct);
            RegisterFeeder<StakedFlipFeeder, StakedFlipInfo>(builder, ct);
            RegisterFeeder<BrokerOverviewFeeder, BrokerOverviewInfo>(builder, ct);
            RegisterFeeder<BigStakedFlipFeeder, BigStakedFlipInfo>(builder, ct);
            RegisterFeeder<BurnFeeder, BurnInfo>(builder, ct);
            RegisterFeeder<DailySwapOverviewFeeder, DailySwapOverviewInfo>(builder, ct);

            builder
                .RegisterType<PriceProvider>()
                .SingleInstance();
            
            builder
                .RegisterType<DiscordConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<TelegramConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<FullTelegramConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<LpTelegramConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<TwitterConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<MastodonConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<DatabaseConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<Runner>()
                .SingleInstance();
            
            builder
                .Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }

        private static void RegisterFeeder<TFeeder, TInfo>(ContainerBuilder builder, CancellationToken ct) where TFeeder : notnull
        {
            builder
                .Register(_ => new Pipeline<TInfo>(new BufferBlock<TInfo>(), ct))
                .SingleInstance();
            
            builder
                .RegisterType<TFeeder>()
                .SingleInstance();
        }
    }
}
