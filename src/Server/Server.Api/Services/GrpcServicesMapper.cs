namespace Server.Api.Services;

public static class GrpcServicesMapper
{
    public static void MapGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<PlayerMainStatsGrpcService>();
    }
}
