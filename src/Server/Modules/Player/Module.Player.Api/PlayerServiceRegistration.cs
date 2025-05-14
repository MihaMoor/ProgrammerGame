using Microsoft.Extensions.DependencyInjection;
using Server.Module.Player.Application;
using Server.Module.Player.Domain;
using Server.Module.Player.Infrastructure;
using Server.Shared.Cqrs;

namespace Server.Module.Player.Api;

public static class PlayerServiceRegistration
{
    public static IServiceCollection AddPlayerServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<MainStatsEventListener>()
            .AddSingleton<IMainStatsChangeNotifier, MainStatsChangeNotifier>()
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
