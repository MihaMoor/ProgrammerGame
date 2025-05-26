using Microsoft.Extensions.DependencyInjection;
using Server.Module.Player.Application;
using Server.Module.Player.Infrastructure;
using Server.Shared.Cqrs;

namespace Server.Module.Player.Api;

public static class PlayerServiceRegistration
{
    public static IServiceCollection AddPlayerServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<PlayerEventListener>()
            .AddSingleton<IPlayerChangeNotifier, PlayerChangeNotifier>()
            .AddScoped<IPlayerChangeNotifier>(sp =>
                sp.GetRequiredService<PlayerChangeNotifier>()
            )
            .AddScoped<IPlayerRepository, PlayerRepository>()
            .AddScoped<
                IQueryHandler<SubscribePlayer, IAsyncEnumerable<Domain.Player>>,
                SubscribePlayerHandler
            >();
    }
}
