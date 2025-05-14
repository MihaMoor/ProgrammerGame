using Elastic.Clients.Elasticsearch;
using Prometheus;
using Serilog;
using Shared.EndpointMapper;

namespace Server.Api;

public class Program
{
    /// <summary>
    /// Entry point for the web application, configuring services, middleware, and starting the server.
    /// </summary>
    /// <param name="args">Command-line arguments for application configuration.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureELK(builder);

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddEndpoints();

        var app = builder.Build();

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
    /// Configures Elasticsearch client and Serilog logging using application settings.
    /// </summary>
    /// <param name="builder">The web application builder used to access configuration and register services.</param>
    /// <exception cref="ArgumentNullException">Thrown if the AppSettings configuration section is missing or null.</exception>
    private static void ConfigureELK(WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
        AppSettings? appSettings = builder.Configuration.Get<AppSettings>();

        if (appSettings == null)
            throw new ArgumentNullException(nameof(appSettings));

        // Создание клиента Elasticsearch
        var elasticsearchOptions = new ElasticsearchClientSettings(
            appSettings.Elasticsearch.GetUri()
        );
        var elasticsearchClient = new ElasticsearchClient(elasticsearchOptions);
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Http(appSettings.Logstash.Url, (long?)appSettings.Logstash.QueueLimitBytes)
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog();
    }
}
