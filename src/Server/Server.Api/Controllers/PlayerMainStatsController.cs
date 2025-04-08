using Microsoft.AspNetCore.Http.HttpResults;
using Shared.GrpcContracts;

namespace Server.Api.Controllers;

public static class PlayerMainStatsController
{
    public static RouteGroupBuilder MapPlayerMainStatsApi(this RouteGroupBuilder app)
    {
        app.MapGet("/", GetAsync);

        return app;
    }

    private static async Task<Ok<PlayerMainStatsDto>> GetAsync()
    {
        var result = new PlayerMainStatsDto()
        {

        };
        await Task.Delay(100);
        return TypedResults.Ok(result);
    }
}
