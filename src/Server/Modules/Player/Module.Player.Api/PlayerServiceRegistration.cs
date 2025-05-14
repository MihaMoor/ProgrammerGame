using Microsoft.Extensions.DependencyInjection;
using Server.Module.Player.Application;
using Server.Module.Player.Domain;
using Server.Module.Player.Infrastructure;
using Server.Shared.Cqrs;

namespace Module.Player.Api;

public static class PlayerServiceRegistration
{
    /// <summary>
    /// Registers player-related services, repositories, and query handlers for main stats into the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which player services will be added.</param>
    /// <returns>The updated service collection with player services registered.</returns>
    public static IServiceCollection AddPlayerServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<MainStatsChangeNotifier>()
            .AddSingleton<MainStatsEventListener>()
            .AddScoped<IMainStatsChangeNotifier>(sp =>
                sp.GetRequiredService<MainStatsChangeNotifier>()
            )
            .AddScoped<IMainStatsRepository, MainStatsRepository>()
            .AddScoped<
                IQueryHandler<SubscribeMainStats, IAsyncEnumerable<MainStats>>,
                SubscribeMainStatsHandler
            >();
    }
}
