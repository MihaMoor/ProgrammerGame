using Elastic.Clients.Elasticsearch;
using Serilog;
using Server.Api.Services;

namespace Server.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Создание клиента Elasticsearch
        var elasticsearchOptions = new ElasticsearchClientSettings(new Uri("http://localhost:9200"));
        var elasticsearchClient = new ElasticsearchClient(elasticsearchOptions);
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Http(
                "http://logstash:5044",
                queueLimitBytes: 104857600 //100 Mb
            )
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGrpcServices();

        app.Run();
    }
}
