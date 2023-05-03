using AirlineSendAgent.App;
using AirlineSendAgent.Configs;
using AirlineSendAgent.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace AirlineSendAgent
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var elasticSearchUrl = Environment.GetEnvironmentVariable("ElasticseearchUrl") ?? config.GetValue<string>("ElasticseearchUrl");

            var rabbitMQConfig = new RabbitMQConfig();
            var rabbitMQSection = config.GetSection("RabbitMQ");

            if (rabbitMQSection["Port"] != null && rabbitMQSection["Host"] != null)
            {
                rabbitMQSection.Bind(rabbitMQConfig);
                Console.WriteLine($"Rabbit MQ docker host - {rabbitMQConfig.Host}");

            } else
            {
                config.GetSection("RabbitMQ:Port").Bind(rabbitMQConfig);
                config.GetSection("RabbitMQ:Host").Bind(rabbitMQConfig);
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "AirlineSendAgent-{0:yyyy.MM.dd}"
                }).CreateLogger();
            // documentation generic host builder
            // var host = HostBuilder()
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog(Log.Logger);

                    loggingBuilder.AddConsole();
                    loggingBuilder.AddDebug();
                })
                .ConfigureServices((context,  services) =>
                {
                    // services.AddSingleton<ILoggerFactory>(new SerilogLoggerFactory(Log.Logger));
                    services.AddSingleton(rabbitMQConfig);
                    services.AddSingleton<Serilog.ILogger>(Log.Logger);
                    services.AddSingleton<IAppHost, AppHost>();

                    // services.AddLogging();

                    services.AddDbContext<SendAgentDbContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("SendAgentConnection") 
                        ?? throw new InvalidOperationException("Connection string 'SendAgentConnection' not found.")));

                    services.AddHostedService<MessageBusWorker>();

                }).Build();

            // host.Services.GetService<IAppHost>().Run();
            // var service = ActivatorUtilities.CreateInstance<AppHost>(host.Services);
            // service.Run();
            // await host.RunConsoleAsync().ConfigureAwait(false);
            await host.RunAsync(); // comment this if you want app to run as normal service
            // host.WaitForShutdown(); // added for dockerization
            // Console.ReadLine();

            // Log.CloseAndFlush();
        }
    }
}