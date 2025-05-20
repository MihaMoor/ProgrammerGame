using Client.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WpfClient.Pages;
using WpfClient.Widgets;

namespace WpfClient;

public static class AppServices
{
    public static ServiceProvider? ServiceProvider { get; private set; }

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
        serviceCollection.AddSingleton<MainWindow>(); // или transient/singleton по требованиям вашего проекта
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
