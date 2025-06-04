using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Shared.EndpointMapper;

namespace Server.Module.Player.Api;

public static class PlayerEndpoint
{
    public sealed class Endpoint : IEndpoint
    {
        /// <summary>
        /// Регистрирует PlayerGrpcService в системе маршрутизации конечных точек приложения.
        /// </summary>
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGrpcService<PlayerGrpcService>();
        }
    }
}
