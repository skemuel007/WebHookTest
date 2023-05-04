using AirlineSendAgentWorker;
// using AirlineSendAgentWorker.App;
using AirlineSendAgentWorker.Configs;
using AirlineSendAgentWorker.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

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

}
else
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


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((context, loggingBuilder) =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(Log.Logger);

        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    })
    .ConfigureServices((context, services) => {

        // services.AddSingleton<ILoggerFactory>(new SerilogLoggerFactory(Log.Logger));

        // services.AddLogging();

        services.AddHostedService<Worker>()
        .AddDbContext<SendAgentDbContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("SendAgentConnection")
                                               ?? throw new InvalidOperationException("Connection string 'SendAgentConnection' not found.")))
        .AddSingleton(rabbitMQConfig)
        .AddSingleton<Serilog.ILogger>(Log.Logger);
        // .AddSingleton<IAppHost, AppHost>();

    })
    .Build();

await host.RunAsync();