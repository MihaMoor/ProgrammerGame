namespace Server.Api.Controllers;

public static class ControllerMapper
{
    public static void MapGrpcControllers(this WebApplication app)
    {
        app.MapGroup("api/grpc/player-main-stats").MapPlayerMainStatsApi();
    }
}
