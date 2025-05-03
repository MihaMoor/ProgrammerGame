using RealTimePrototype.API.HostedServices;
using RealTimePrototype.Domain.Abstractions;
using RealTimePrototype.Domain.Services;
using RealTimePrototype.Infrastructure.Repositories;

namespace RealTimePrototype.DI;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        => services
            .AddSingleton<IPlayerRepository, PlayerInMemoryRepository>()
            .AddSingleton<HungryService>();

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
        => services.AddHostedService<TimedHostedService>();
}
