using Microsoft.AspNetCore.Routing;

namespace Shared.EndpointMapper;

public interface IEndpoint
{
    /// <summary>
/// Configures endpoint mappings using the provided route builder.
/// </summary>
/// <param name="app">The route builder to which endpoints will be mapped.</param>
void MapEndpoints(IEndpointRouteBuilder app);
}
