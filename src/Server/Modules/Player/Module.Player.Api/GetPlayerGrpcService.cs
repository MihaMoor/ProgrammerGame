using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using Server.Module.Player.Domain;
using Server.Module.Player.GrpcContracts;
using Server.Shared.Cqrs;
using Server.Shared.Results;

namespace Server.Module.Player.Api;

internal class GetPlayerGrpcService(
    ILogger<GetPlayerGrpcService> logger,
    IQueryHandler<GetMainStatsQuery, MainStats> handler
) : PlayerService.PlayerServiceBase
{
    public override async Task<PlayerDto> Get(UUID request, ServerCallContext context)
    {
        GetMainStatsQuery query = new(new(request.Id));

        Result<MainStats> result = await handler.Handle(query);

        if (result.IsFailure)
        {
            logger.LogError(result.Error.ToString());
            throw new RpcException(new Status(StatusCode.NotFound, result.Error.ToString()));
        }

        return result.Value.ToViewModel();
    }
}

internal static class PlayerExtensions
{
    public static PlayerDto ToViewModel(this MainStats stats) =>
        new()
        {
            Name = stats.Name,
            Health = stats.Health,
            Hunger = stats.Hunger,
            PocketMoney = stats.PocketMoney,
            Mood = stats.Mood,
        };
}
