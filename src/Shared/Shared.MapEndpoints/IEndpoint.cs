using Microsoft.AspNetCore.Routing;

namespace Shared.EndpointMapper;

public interface IEndpoint
{
    void MapEndpoints(IEndpointRouteBuilder app);
}
