using Client.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WpfClient.Pages;
using WpfClient.Widgets;

namespace WpfClient;

public static class AppServices
{
    public static ServiceProvider? ServiceProvider { get; private set; }

    /// <summary>
    /// Настраивает и создает поставщик сервисов dependency injection приложения со всеми необходимыми сервисами,
    /// gRPC-клиентами, страницами и виджетами.
    /// </summary>
    public static void Configure()
    {
        ServiceCollection serviceCollection = new();

        serviceCollection
            .ConfigureServices()
            .ConfigureGrpcServices("https://localhost:8081")
            .ConfigurePages()
            .ConfigureWidgets();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public static ServiceCollection ConfigureServices(this ServiceCollection serviceCollection)
    {
        return serviceCollection;
    }

    private static ServiceCollection ConfigurePages(this ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddScoped<Game>();

        return serviceCollection;
    }

    private static ServiceCollection ConfigureWidgets(this ServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<MenuBar>();
        serviceCollection.AddScoped<PlayerWidget>();

        return serviceCollection;
    }
}
