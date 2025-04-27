namespace RealTimePrototype.API.Controllers;

public static class ControllersMapper
{
    public static WebApplication MapControllers(this WebApplication app, string prefix)
    {
        app
            .MapGroup(prefix)
            .MapPlayers();

        return app;
    }
}
