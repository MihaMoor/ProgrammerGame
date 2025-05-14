using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Shared.EndpointMapper;

public static class EndpointExtensions
{
    /// <summary>
    /// Registers all types implementing <c>IEndpoint</c> from the entry assembly, its referenced assemblies, and the executing assembly as transient services.
    /// </summary>
    /// <returns>The modified <see cref="IServiceCollection"/> with registered endpoint services.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();
        List<Assembly> referencedAssemblies =
            entryAssembly?.GetReferencedAssemblies().Select(Assembly.Load).ToList() ?? [];

        // Добавляем текущую сборку и сборку приложения
        referencedAssemblies.Add(Assembly.GetExecutingAssembly());
        if (entryAssembly != null)
            referencedAssemblies.Add(entryAssembly);

        foreach (var assembly in referencedAssemblies)
        {
            services.AddEndpoints(assembly);
        }

        return services;
    }

    /// <summary>
    /// Registers all non-abstract, non-interface types implementing <see cref="IEndpoint"/> from the specified assembly as transient services.
    /// </summary>
    /// <param name="assembly">The assembly to scan for <see cref="IEndpoint"/> implementations.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddEndpoints(
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
    /// Maps all registered <see cref="IEndpoint"/> implementations to the application's routing pipeline using the specified route group builder or the application itself.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map endpoints onto.</param>
    /// <param name="routeGroupBuilder">An optional <see cref="RouteGroupBuilder"/> to group mapped endpoints; if null, endpoints are mapped directly to the application.</param>
    /// <returns>The <see cref="WebApplication"/> instance with endpoints mapped.</returns>
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
