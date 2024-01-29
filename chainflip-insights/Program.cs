namespace ChainflipInsights
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Discord;
    using Discord.WebSocket;
    using ChainflipInsights.Configuration;
    using ChainflipInsights.Infrastructure;
    using ChainflipInsights.Infrastructure.Options;
    using ChainflipInsights.Modules;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;

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
            
            var container = ConfigureServices(configuration);
            var logger = container.GetRequiredService<ILogger<Program>>();
            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name;
            
            logger.LogInformation(
                "Starting {ApplicationName}",
                applicationName);
            
            Console.CancelKeyPress += (_, eventArgs) =>
            { 
                logger.LogInformation("Requesting disconnect...");
                CancellationTokenSource.Cancel();
                eventArgs.Cancel = true;
            };

            try
            {
                #if DEBUG
                Console.WriteLine($"Press ENTER to start {applicationName}...");
                Console.ReadLine();
                #endif
                
                // Run the bot
                var bot = container.GetRequiredService<Bot>();
                var botTask = bot.RunAsync(ct);
                
                Console.WriteLine("Running... Press CTRL + C to exit.");
                botTask.GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Encountered a fatal exception, exiting program.");

                throw;
            }
            
            logger.LogInformation("Stopping...");
            
            // Allow some time for flushing before shutdown.
            Log.CloseAndFlush();
            Thread.Sleep(1000);
        }

        private static AutofacServiceProvider ConfigureServices(
            IConfiguration configuration)
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
                .RegisterType<Bot>()
                .SingleInstance();
            
            builder
                .Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }
    }
}
