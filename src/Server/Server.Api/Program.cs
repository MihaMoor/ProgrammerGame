using Elastic.Channels;
using Elastic.Clients.Elasticsearch;
using Elastic.Serilog.Sinks;
using Serilog;
using Serilog.Core;
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
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticsearchClient.Transport)
            {
                // Настройки Elasticsearch
                MinimumLevel = Serilog.Events.LogEventLevel.Debug,
                ConfigureChannel = channelOptions =>
                {
                    channelOptions.BufferOptions = new BufferOptions();
                },
                LevelSwitch = new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Debug)
            })
            //.WriteTo.Elasticsearch([new Uri("http://localhost:9200")],
            //    options =>
            //    {
            //        options.DataStream = new DataStreamName() { Name = "ProgrammingGame" };
            //        options.TextFormatting = new EcsTextFormatterConfiguration();
            //        options.BootstrapMethod = BootstrapMethod.Failure;
            //        options.ConfigureChannel = channelOptions =>
            //        {
            //            channelOptions.BufferOptions = new BufferOptions();
            //        };
            //        options.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
            //    },
            //    configureTransport =>
            //    {
            //        configureTransport.ServerCertificateValidationCallback((_, _, _, _) => true);
            //    })
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
