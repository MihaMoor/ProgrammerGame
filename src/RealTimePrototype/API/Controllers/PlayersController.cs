using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RealTimePrototype.API.Dtos.Players;
using RealTimePrototype.Domain.Abstractions;
using RealTimePrototype.Domain.Aggregates;
using RealTimePrototype.Domain.Services;

namespace RealTimePrototype.API.Controllers;

public static class PlayersController
{
    public static RouteGroupBuilder MapPlayers(this RouteGroupBuilder rootRoute)
    {
        var route = rootRoute.MapGroup("players");

        route.MapGet("{id:int}", Get);
        route.MapGet("{id:int}/satiety", GetSatiety);
        route.MapGet("{id:int}/activate", Activate);
        route.MapGet("{id:int}/deactivate", Deactivate);
        route.MapGet("{id:int}/test/hangry", Hungry);

        route.MapPost("/", Create);

        return rootRoute;
    }

    private static Results<Ok<PlayerDto>, NotFound> Get(int id, IPlayerRepository repository)
    {
        Player? player = repository.GetById(id);

        if (player == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(player.FromDomain());
    }

    private static Results<Ok<float>, NotFound> GetSatiety(int id, IPlayerRepository repository)
    {
        Player? player = repository.GetById(id);

        if (player == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(player.Satiety);
    }

    private static Ok Activate(int id, HungryService service)
    {
        service.Activate(id);

        return TypedResults.Ok();
    }

    private static Ok Deactivate(int id, HungryService service)
    {
        service.Deactivate(id);

        return TypedResults.Ok();
    }

    private static Ok Hungry(int id, HungryService service)
    {
        service.Hungry();

        return TypedResults.Ok();
    }

    private static Results<Created, NoContent, BadRequest> Create(
        [FromBody] CreatePlayerDto command,
        [FromServices] IPlayerRepository repository)
    {
        Player? player = repository.GetById(command.Id);

        if (player != null)
            return TypedResults.NoContent();

        player = new()
        {
            Id = command.Id,
            Satiety = command.Satiety,
        };

        if (!repository.Create(player))
            return TypedResults.BadRequest();

        return TypedResults.Created($"/{player.Id}");
    }
}
