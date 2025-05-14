using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Shared.EndpointMapper;

namespace Server.Module.Player.Api;

public static class PlayerEndpoint
{
    public sealed class Enpoint : IEndpoint
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGrpcService<GetPlayerGrpcService>();
            app.MapGrpcService<CreatePlayerGrpcService>();
            app.MapGrpcService<SubscribePlayerGrpcService>();
        }
    }
}
