using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Module.Player.Application;
using Server.Module.Player.Infrastructure;
using Server.Module.Player.Infrastructure.EfCore;
using Server.Shared.Cqrs;

namespace Server.Module.Player.Api;

public static class PlayerServiceRegistration
{
    /// <summary>
    /// Registers player-related services and configures the player database context using settings from the provided configuration.
    /// </summary>
    /// <param name="configuration">Application configuration containing the "PlayerSettings" section.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the "PlayerSettings" configuration section is missing or invalid.</exception>
    public static IServiceCollection AddPlayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PlayerSettings>(configuration.GetRequiredSection("PlayerSettings"));
        PlayerSettings playerSettings = configuration.GetRequiredSection("PlayerSettings").Get<PlayerSettings>() ??
            throw new InvalidOperationException("PlayerSettings configuration is required");

        return services
            .AddDbContextPool<Context>(options =>
            {
                options.UseNpgsql(playerSettings.PostgreSql.CreateConnectionString());
            })
            .AddSingleton<IPlayerChangeNotifier, PlayerChangeNotifier>()
            .AddSingleton<PlayerEventListener>()
            .AddTransient<IPlayerRepository, PlayerRepository>()
            .AddTransient<
                IQueryHandler<SubscribePlayer, IAsyncEnumerable<Domain.Player>>,
                SubscribePlayerHandler
            >()
            .AddTransient<IQueryHandler<GetPlayerQuery, Domain.Player>, GetPlayerQueryHandler>();
    }
}
