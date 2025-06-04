using Elastic.Clients.Elasticsearch;
using Prometheus;
using Serilog;
using Server.Module.Player.Api;
using Shared.EndpointMapper;
using System.Reflection;

namespace Server.Api;

public class Program
{
    private const ulong DefaultQueueLimitBytes = 104_857_600UL;

    /// <summary>
    /// Настраивает и запускает веб-приложение, включая настройку служб, логирования, gRPC, OpenAPI, конечных точек и промежуточного программного обеспечения.
    /// </summary>
    /// <param name="args">Аргументы командной строки для конфигурации приложения.</param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        LoadAllReferencedAssemblies();

        ConfigureELK(builder);

        // Add services to the container.
        builder.Services.AddGrpc(options =>
        {
            options.IgnoreUnknownServices = true; // Отключает "Unimplemented service"
            options.EnableDetailedErrors = true;
        });
        builder.Services.AddPlayerServices(builder.Configuration);

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
    /// Настраивает Elasticsearch и логирование Serilog для приложения, используя параметры из раздела конфигурации "AppSettings".
    /// </summary>
    /// <remarks>
    /// Если раздел "AppSettings" отсутствует, логирование настраивается только для вывода в консоль. В противном случае этот метод инициализирует клиента Elasticsearch и настраивает Serilog для записи логов как в консоль, так и в HTTP-ендпоинт Logstash, с учетом лимитов очереди, указанных в конфигурации. Клиент Elasticsearch регистрируется как синглтон-сервис.
    /// </remarks>
    private static void ConfigureELK(WebApplicationBuilder builder)
    {
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

    /// <summary>
    /// Загружает все DLL-ассамблеи из базовой директории приложения и их зависимости, которые еще не загружены в текущий AppDomain.
    /// </summary>
    /// <remarks>
    /// Этот метод обеспечивает наличие всех управляемых сборок и их зависимостей во время выполнения, динамически загружая отсутствующие DLL из каталога приложения. Ошибки, возникающие при загрузке, выводятся в консоль.
    /// </remarks>
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
