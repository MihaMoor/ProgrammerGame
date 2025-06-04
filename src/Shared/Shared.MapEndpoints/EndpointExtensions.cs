using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Shared.EndpointMapper;

public static class EndpointExtensions
{
    /// <summary>
    /// Registers all implementations of <see cref="IEndpoint"/> found in the currently loaded assemblies as transient services.
    /// </summary>
    /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            services.AddEndpointsFromAssembly(assembly);
        }

        return services;
    }

    /// <summary>
    /// Registers all non-abstract, non-interface types implementing <see cref="IEndpoint"/> from the specified assembly as transient services.
    /// </summary>
    /// <param name="assembly">The assembly to scan for <see cref="IEndpoint"/> implementations.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection AddEndpointsFromAssembly(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes.Where(type =>
                type is { IsAbstract: false, IsInterface: false }
                && type.IsAssignableTo(typeof(IEndpoint))
            )
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    /// Maps all registered <see cref="IEndpoint"/> implementations to the application's routing system.
    /// </summary>
    /// <param name="routeGroupBuilder">
    /// Optional route group builder to use for endpoint mapping. If not provided, the application's route builder is used.
    /// </param>
    /// <returns>The <see cref="WebApplication"/> instance for chaining.</returns>
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null
    )
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<
            IEnumerable<IEndpoint>
        >();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoints(builder);
        }

        return app;
    }
}
