using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Server.Api;

namespace Server.Module.Player.Infrastructure.EfCore.Migrations;

public class Programm()
{
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
