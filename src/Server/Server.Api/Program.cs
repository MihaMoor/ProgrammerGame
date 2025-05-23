using Elastic.Clients.Elasticsearch;
using Prometheus;
using Serilog;
using Shared.EndpointMapper;

namespace Server.Api;

public class Program
{
    private const ulong DefaultQueueLimitBytes = 104_857_600UL;

    /// <summary>
    /// Entry point for the ASP.NET Core application, configuring services, middleware, logging, and starting the web server.
    /// </summary>
    /// <param name="args">Command-line arguments for application configuration.</param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        ConfigureELK(builder);

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddEndpoints();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapMetrics();
        app.MapEndpoints();

        app.Run();
    }

    /// <summary>
    /// Configures logging and Elasticsearch integration for the application using settings from the "AppSettings" configuration section.
    /// </summary>
    /// <remarks>
    /// If "AppSettings" is not present, sets up Serilog to log to the console only. Otherwise, initializes an Elasticsearch client and configures Serilog to log to both the console and a Logstash HTTP endpoint, applying a queue size limit. Registers the Elasticsearch client as a singleton service.
    /// </remarks>
    private static void ConfigureELK(WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
        AppSettings? appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

        if (appSettings == null)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog();
            return;
        }

        // Создание клиента Elasticsearch
        ElasticsearchClientSettings elasticsearchOptions = new(
            appSettings.Elasticsearch?.GetUri() ?? new Uri("http://localhost:9200")
        );
        ElasticsearchClient elasticsearchClient = new(elasticsearchOptions);

        ulong queueLimitBytes = appSettings.Logstash?.QueueLimitBytes ?? DefaultQueueLimitBytes;

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Http(
                appSettings.Logstash?.Url ?? "http://logstash:5044",
                (long?)(queueLimitBytes is > long.MaxValue ? long.MaxValue : (long)queueLimitBytes)
                    ?? 104_857_600
            )
            .CreateLogger();
        builder.Services.AddSingleton(elasticsearchClient);
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog();
    }
}
