using Elastic.Clients.Elasticsearch;
using Prometheus;
using Serilog;
using Shared.EndpointMapper;
using System.Reflection;

namespace Server.Api;

public class Program
{
    private const ulong DefaultQueueLimitBytes = 104_857_600UL;

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        LoadAllReferencedAssemblies();

        ConfigureELK(builder);

        // Add services to the container.
        builder.Services.AddGrpc(options =>
        {
            options.IgnoreUnknownServices = true; // Отключает "Unimplemented service"
            options.EnableDetailedErrors = true;
        });
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

    private static void LoadAllReferencedAssemblies()
    {
        List<Assembly> loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        string[] loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

        // Ищем все DLL в папке приложения
        string[] referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
        List<string> toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

        toLoad.ForEach(path =>
        {
            try
            {
                // Загружаем сборку
                Assembly assembly = Assembly.LoadFrom(path);

                // Загружаем все её зависимости
                foreach (var referenced in assembly.GetReferencedAssemblies())
                {
                    if (!loadedAssemblies.Any(a => a.FullName == referenced.FullName))
                    {
                        Assembly.Load(referenced);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки сборки {path}: {ex.Message}");
            }
        });
    }
}
