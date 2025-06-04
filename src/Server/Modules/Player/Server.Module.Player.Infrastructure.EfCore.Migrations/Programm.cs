using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Server.Api;

namespace Server.Module.Player.Infrastructure.EfCore.Migrations;

public class Programm()
{
    /// <summary>
    /// Entry point for the application that loads configuration from "appsettings.json" and validates the presence of the "AppSettings" section.
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
    /// Creates a new <see cref="Context"/> instance configured with settings from "appsettings.json" for design-time operations.
    /// </summary>
    /// <param name="args">Command-line arguments (not used).</param>
    /// <returns>A configured <see cref="Context"/> for use with Entity Framework Core tools.</returns>
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
