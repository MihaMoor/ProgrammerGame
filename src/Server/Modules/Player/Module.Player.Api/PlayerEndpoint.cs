using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Shared.EndpointMapper;

namespace Server.Module.Player.Api;

public static class PlayerEndpoint
{
    public sealed class Enpoint : IEndpoint
    {
        /// <summary>
        /// Registers the gRPC services for retrieving and creating players with the application's endpoint routing.
        /// </summary>
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGrpcService<GetPlayerGrpcService>();
            app.MapGrpcService<CreatePlayerGrpcService>();
        }
    }
}
