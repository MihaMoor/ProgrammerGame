using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RealTimePrototype.API.Dtos.Players;
using RealTimePrototype.Domain.Abstractions;
using RealTimePrototype.Domain.Aggregates;
using RealTimePrototype.Domain.Services;

namespace RealTimePrototype.API.Endpoints;

public static class PlayerEndpoints
{
    public static RouteGroupBuilder MapPlayers(this RouteGroupBuilder rootRoute)
    {
        var route = rootRoute.MapGroup("players");

        route.MapGet("{id:int}", Get);
        //route.MapGet("{id:int}/satiety", GetSatiety);
        route.MapGet("{id:int}/activate", Activate);
        route.MapGet("{id:int}/deactivate", Deactivate);
        //route.MapGet("{id:int}/test/hangry", Hungry);

        route.MapPost("/", Create);

        route.MapPut("{id:int}", Update);

        return rootRoute;
    }

    private static Results<Ok<PlayerDto>, NotFound> Get(int id, [FromServices] IPlayerRepository repository)
    {
        Player? player = repository.GetById(id);

        if (player == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(player.FromDomain());
    }

    //private static Results<Ok<float>, NotFound> GetSatiety(int id, IPlayerRepository repository)
    //{
    //    Player? player = repository.GetById(id);

    //    if (player == null)
    //        return TypedResults.NotFound();

    //    return TypedResults.Ok(player.Satiety);
    //}

    private static Ok Activate(int id, [FromServices] HungryService service)
    {
        service.Activate(id);

        return TypedResults.Ok();
    }

    private static Ok Deactivate(int id, [FromServices] HungryService service)
    {
        service.Deactivate(id);

        return TypedResults.Ok();
    }

    //private static Ok Hungry(int id, HungryService service)
    //{
    //    service.Hungry();

    //    return TypedResults.Ok();
    //}

    private static Results<Created, NoContent, BadRequest> Create(
        [FromBody] CreatePlayerDto command,
        [FromServices] IPlayerRepository repository)
    {
        Player? player = repository.GetById(command.Id);

        if (player != null)
            return TypedResults.NoContent();

        player = command.ToPlayerDomain();

        if (!repository.Create(player))
            return TypedResults.BadRequest();

        return TypedResults.Created($"/{player.Id}");
    }

    private static Results<NoContent, BadRequest, NotFound> Update(
        int id,
        [FromBody] UpdatePlayerDto command,
        [FromServices] IPlayerRepository repository)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        Player? player = repository.GetById(command.Id);

        if (player == null)
            return TypedResults.NotFound();

        Player updatedPlayer = command.ToPlayerDomain();

        if (!repository.Update(updatedPlayer))
            return TypedResults.BadRequest();

        return TypedResults.NoContent();
    }
}
