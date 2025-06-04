using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Server.Api;

namespace Server.Module.Player.Infrastructure.EfCore.Migrations;

public class Programm()
{
    /// <summary>
    /// Точка входа в приложение, которая загружает конфигурацию из файла "appsettings.json" и проверяет наличие раздела "AppSettings".
    /// </summary>
    public static void Main()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = builder.Build();

        AppSettings? settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();

        if (settings is null)
        {
            Console.WriteLine("App settings not found!");
            return;
        }
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Context>
{
    /// <summary>
    /// Создает новый экземпляр <see cref="Context"/>, настроенный с использованием настроек из файла "appsettings.json" для операций проектирования.
    /// </summary>
    /// <param name="args">Аргументы командной строки (не используются).</param>
    /// <returns>Настроенный экземпляр <see cref="Context"/> для использования с инструментами Entity Framework Core.</returns>
    public Context CreateDbContext(string[] args)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = builder.Build();

        AppSettings? settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();

        //if (settings is null)
        //{
        //    Console.WriteLine("App settings not found!");
        //    return;
        //}

        DbContextOptionsBuilder<Context> optionsBuilder = new();
        optionsBuilder.UseNpgsql(settings?.PostgreSql.CreateConnectionString());
        return new Context(optionsBuilder.Options);
    }
}
