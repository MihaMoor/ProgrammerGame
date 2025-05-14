using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Shared.EndpointMapper;

public static class EndpointExtensions
{
    /// <summary>
    /// Registers all non-abstract types implementing <c>IEndpoint</c> from the entry assembly, its referenced assemblies, and the executing assembly as transient services.
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
    /// Registers all non-abstract types in the specified assembly that implement <see cref="IEndpoint"/> as transient services.
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
    /// Maps all registered <see cref="IEndpoint"/> instances to the application's routing pipeline using the provided route group builder or the application itself.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to map endpoints on.</param>
    /// <param name="routeGroupBuilder">
    /// Optional. The <see cref="RouteGroupBuilder"/> to use for mapping endpoints. If null, the application is used as the route builder.
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
