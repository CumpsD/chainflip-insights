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
    using ChainflipInsights.Consumers.Discord;
    using ChainflipInsights.Consumers.Telegram;
    using ChainflipInsights.Consumers.Twitter;
    using ChainflipInsights.Feeders.Epoch;
    using ChainflipInsights.Feeders.Funding;
    using ChainflipInsights.Feeders.Redemption;
    using ChainflipInsights.Feeders.Liquidity;
    using ChainflipInsights.Feeders.Swap;
    using ChainflipInsights.Infrastructure;
    using ChainflipInsights.Infrastructure.Options;
    using ChainflipInsights.Infrastructure.Pipelines;
    using ChainflipInsights.Modules;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Telegram.Bot;
    using Tweetinvi;

    public class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new();
        
        private static Pipeline<SwapInfo>? _swapPipeline;
        private static Pipeline<IncomingLiquidityInfo>? _incomingLiquidityPipeline;
        private static Pipeline<EpochInfo>? _epochPipeline;
        private static Pipeline<FundingInfo>? _fundingPipeline;
        private static Pipeline<RedemptionInfo>? _redemptionPipeline;

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
            
            logger.LogInformation(
                "Starting {ApplicationName}",
                applicationName);
            
            Console.CancelKeyPress += (_, eventArgs) =>
            { 
                logger.LogInformation("Requesting stop...");
                
                _swapPipeline?.Source.Complete();
                _incomingLiquidityPipeline?.Source.Complete();
                _epochPipeline?.Source.Complete();
                _fundingPipeline?.Source.Complete();
                _redemptionPipeline?.Source.Complete();

                CancellationTokenSource.Cancel();

                eventArgs.Cancel = true;
            };

            try
            {
                #if DEBUG
                Console.WriteLine($"Press ENTER to start {applicationName}...");
                Console.ReadLine();
                #endif
                
                _swapPipeline = container.GetRequiredService<Pipeline<SwapInfo>>();
                _incomingLiquidityPipeline = container.GetRequiredService<Pipeline<IncomingLiquidityInfo>>();
                _epochPipeline = container.GetRequiredService<Pipeline<EpochInfo>>();
                _fundingPipeline = container.GetRequiredService<Pipeline<FundingInfo>>();
                _redemptionPipeline = container.GetRequiredService<Pipeline<RedemptionInfo>>();

                var runner = container.GetRequiredService<Runner>();

                var swapFeeder = container.GetRequiredService<SwapFeeder>();
                var incomingLiquidityFeeder = container.GetRequiredService<IncomingLiquidityFeeder>();
                var epochFeeder = container.GetRequiredService<EpochFeeder>();
                var fundingFeeder = container.GetRequiredService<FundingFeeder>();
                var redemptionFeeder = container.GetRequiredService<RedemptionFeeder>();

                var tasks = new List<Task>();
                tasks.AddRange(runner.Start());
                
                tasks.Add(swapFeeder.Start());
                tasks.Add(incomingLiquidityFeeder.Start());
                tasks.Add(epochFeeder.Start());
                tasks.Add(fundingFeeder.Start());
                tasks.Add(redemptionFeeder.Start());

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

            builder
                .RegisterModule(new LoggingModule(configuration, services));

            var botConfiguration = configuration
                .GetSection(BotConfiguration.Section)
                .Get<BotConfiguration>()!;
            
            services
                .ConfigureAndValidate<BotConfiguration>(configuration.GetSection(BotConfiguration.Section))
                                    
                .AddHttpClient(
                    "Graph",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.GraphUrl);
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    })
                
                .Services
                
                .AddHttpClient(
                    "Swap",
                    x =>
                    {
                        x.BaseAddress = new Uri(botConfiguration.SwapUrl);
                        x.DefaultRequestHeaders.UserAgent.ParseAdd("discord-chainflip-insights");
                    });;
            
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
                .Register(_ => new Pipeline<SwapInfo>(new BufferBlock<SwapInfo>(), ct))
                .SingleInstance();

            builder
                .Register(_ => new Pipeline<IncomingLiquidityInfo>(new BufferBlock<IncomingLiquidityInfo>(), ct))
                .SingleInstance();
            
            builder
                .Register(_ => new Pipeline<EpochInfo>(new BufferBlock<EpochInfo>(), ct))
                .SingleInstance();
            
            builder
                .Register(_ => new Pipeline<FundingInfo>(new BufferBlock<FundingInfo>(), ct))
                .SingleInstance();
            
            builder
                .Register(_ => new Pipeline<RedemptionInfo>(new BufferBlock<RedemptionInfo>(), ct))
                .SingleInstance();
            
            builder
                .RegisterType<SwapFeeder>()
                .SingleInstance();
            
            builder
                .RegisterType<IncomingLiquidityFeeder>()
                .SingleInstance();
            
            builder
                .RegisterType<EpochFeeder>()
                .SingleInstance();
            
            builder
                .RegisterType<FundingFeeder>()
                .SingleInstance();
            
            builder
                .RegisterType<RedemptionFeeder>()
                .SingleInstance();
            
            builder
                .RegisterType<DiscordConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<TelegramConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<TwitterConsumer>()
                .SingleInstance();
            
            builder
                .RegisterType<Runner>()
                .SingleInstance();
            
            builder
                .Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }
    }
}
