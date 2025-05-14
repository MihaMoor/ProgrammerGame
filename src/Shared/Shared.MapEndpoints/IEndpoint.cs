using Microsoft.AspNetCore.Routing;

namespace Shared.EndpointMapper;

public interface IEndpoint
{
    /// <summary>
/// Configures and maps endpoints onto the specified route builder.
/// </summary>
/// <param name="app">The route builder to which endpoints will be mapped.</param>
void MapEndpoints(IEndpointRouteBuilder app);
}
