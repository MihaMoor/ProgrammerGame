using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Shared.EndpointMapper;

namespace Server.Module.Player.Api;

public static class PlayerEndpoint
{
    public sealed class Endpoint : IEndpoint
    {
        /// <summary>
        /// Registers the PlayerGrpcService with the application's endpoint routing system.
        /// </summary>
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGrpcService<PlayerGrpcService>();
        }
    }
}
