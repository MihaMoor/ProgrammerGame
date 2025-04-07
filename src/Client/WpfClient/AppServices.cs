using Client.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WpfClient.Pages;
using WpfClient.Widgets;
using WpfClient.Widgets.MainStats;

namespace WpfClient;

public static class AppServices
{
    public static ServiceProvider? ServiceProvider { get; private set; }

    public static void Configure()
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        GrpsServices.ConfigureGrpcServices(serviceCollection, "https://localhost:5001");

        ConfigurePages(serviceCollection);
        ConfigureWidgets(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
    }

    private static void ConfigurePages(ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MainWindow>(); // или transient/singleton по требованиям вашего проекта
        serviceCollection.AddScoped<Game>();
    }

    private static void ConfigureWidgets(ServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<MenuBar>();
        serviceCollection.AddScoped<MainStats>();
    }
}