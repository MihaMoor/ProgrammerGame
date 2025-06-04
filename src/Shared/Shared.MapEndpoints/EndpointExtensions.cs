using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Shared.EndpointMapper;

public static class EndpointExtensions
{
    /// <summary>
    /// Регистрирует все реализации <see cref="IEndpoint"/>, найденные в загруженных в данный момент сборках, в качестве временных служб (transient).
    /// </summary>
    /// <returns>Объект <see cref="IServiceCollection"/> с добавленными службами для цепочного вызова.</returns>
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
    /// Регистрирует все нефреймворковые типы, реализующие <see cref="IEndpoint"/>, из указанной сборки как временные службы (transient).
    /// Типы должны быть не абстрактными и не интерфейсами.
    /// </summary>
    /// <param name="assembly">Сборка, в которой осуществляется поиск реализаций <see cref="IEndpoint"/>.</param>
    /// <returns>Объект <see cref="IServiceCollection"/> с добавленными службами для дальнейшего использования и цепочного вызова.</returns>
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
    /// Регистрирует все зарегистрированные реализации <see cref="IEndpoint"/> в системе маршрутизации приложения.
    /// </summary>
    /// <param name="routeGroupBuilder">
    /// Необязательный объект для группировки маршрутов. Если не предоставлен, используется основной маршрутизатор приложения.
    /// </param>
    /// <returns>Экземпляр <see cref="WebApplication"/> для цепочного вызова методов.</returns>
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
