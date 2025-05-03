namespace RealTimePrototype.API.Endpoints;

public static class EndpointsMapper
{
    public static WebApplication MapEndpoints(this WebApplication app, string prefix)
    {
        app
            .MapGroup(prefix)
            .MapPlayers();

        return app;
    }
}
