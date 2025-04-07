using Microsoft.Extensions.DependencyInjection;

namespace WpfClient;

public static class AppServices
{
    public static ServiceProvider ServiceProvider { get; private set; }

    public static void ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        // Регистрация зависимостей
        serviceCollection.AddSingleton<MainWindow>(); // или transient/singleton по требованиям вашего проекта
        //serviceCollection.AddTransient<IGreeterClient>(sp => new GreeterClient("https://localhost:5001"));

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}