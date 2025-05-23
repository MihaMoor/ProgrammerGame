using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Shared.EndpointMapper;

namespace Server.Module.Player.Api;

public static class PlayerEndpoint
{
    public sealed class Endpoint : IEndpoint
    {
        /// <summary>
        /// Registers player-related gRPC services with the application's endpoint routing system.
        /// </summary>
        /// <param name="app">The endpoint route builder used to configure service endpoints.</param>
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGrpcService<GetPlayerGrpcService>();
            app.MapGrpcService<CreatePlayerGrpcService>();
            app.MapGrpcService<SubscribePlayerGrpcService>();
        }
    }
}
