using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Shared.EndpointMapper;

public static class EndpointExtensions
{
    /// <summary>
    /// ������������ ��� ���������� <see cref="IEndpoint"/>, ��������� � ����������� � ������ ������ �������, � �������� ��������� ����� (transient).
    /// </summary>
    /// <returns>������ <see cref="IServiceCollection"/> � ������������ �������� ��� ��������� ������.</returns>
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
    /// ������������ ��� ��������������� ����, ����������� <see cref="IEndpoint"/>, �� ��������� ������ ��� ��������� ������ (transient).
    /// ���� ������ ���� �� ������������ � �� ������������.
    /// </summary>
    /// <param name="assembly">������, � ������� �������������� ����� ���������� <see cref="IEndpoint"/>.</param>
    /// <returns>������ <see cref="IServiceCollection"/> � ������������ �������� ��� ����������� ������������� � ��������� ������.</returns>
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
    /// ������������ ��� ������������������ ���������� <see cref="IEndpoint"/> � ������� ������������� ����������.
    /// </summary>
    /// <param name="routeGroupBuilder">
    /// �������������� ������ ��� ����������� ���������. ���� �� ������������, ������������ �������� ������������� ����������.
    /// </param>
    /// <returns>��������� <see cref="WebApplication"/> ��� ��������� ������ �������.</returns>
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
