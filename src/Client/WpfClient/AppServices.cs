using Client.Infrastructure;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using WpfClient.Pages;
using WpfClient.Widgets;
using WpfClient.Widgets.MainStats;

namespace WpfClient;

public static class AppServices
{
    public static ServiceProvider ServiceProvider { get; private set; }

    public static void Configure()
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);
        ConfigurePages(serviceCollection);
        ConfigureWidgets(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
        //serviceCollection.AddTransient<IGreeterClient>(sp => new GreeterClient("https://localhost:5001"));
        serviceCollection.AddSingleton(x => GrpcChannel.ForAddress("https://localhost:5001"));
        serviceCollection.AddScoped(x => new Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient(x.GetRequiredService<GrpcChannel>()));
        serviceCollection.AddScoped
            ( x =>
                {
                    var client = x.GetRequiredService<Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient>();
                    return new GrpcClient<Empty, Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient>("https://localhost:5001", client);
                }
            );
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