using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Shared.EndpointMapper;

public static class EndpointExtensions
{
    /// <summary>
    /// Registers all <c>IEndpoint</c> implementations from all loaded assemblies into the service collection as transient services.
    /// </summary>
    /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
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
    /// <returns>The updated service collection.</returns>
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
    /// Maps all registered <see cref="IEndpoint"/> implementations to the application's routing pipeline.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <param name="routeGroupBuilder">
    /// Optional <see cref="RouteGroupBuilder"/> to use for mapping endpoints. If null, the application itself is used.
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
