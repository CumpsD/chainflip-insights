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
                var swapRunner = container.GetRequiredService<SwapRunner>();
                var swapFeeder = container.GetRequiredService<SwapFeeder>();

                var tasks = new List<Task>();
                tasks.AddRange(swapRunner.Start());
                tasks.Add(swapFeeder.Start());
                
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
                .Register(_ =>
                {
                    var source = new BufferBlock<SwapInfo>();

                    return new Pipeline<SwapInfo>(source, ct);
                })
                .SingleInstance();

            builder
                .RegisterType<SwapFeeder>()
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
                .RegisterType<SwapRunner>()
                .SingleInstance();


            
            builder
                .Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }
    }
}
