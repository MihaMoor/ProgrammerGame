using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using Server.Module.Player.Domain;
using Server.Module.Player.GrpcContracts;

namespace Server.Module.Player.Api;

internal class GetPlayerGrpcService(
    ILogger<GetPlayerGrpcService> logger,
    IQueryHandler<GetMainStatsQuery, MainStats> handler
) : PlayerService.PlayerServiceBase
{
    public override async Task Get(
        UUID request,
        IServerStreamWriter<PlayerDto> responseStream,
        ServerCallContext context
    )
    {
        GetMainStatsQuery query = new(new(request.Id));

        Result<MainStats> result = await handler.Handle(query);

        if (result.IsFailure)
        {
            logger.LogError(result.Error.ToString());
            await responseStream.WriteAsync(new PlayerDto());
        }

        await responseStream.WriteAsync(result.Value.ToViewModel());

        while (!context.CancellationToken.IsCancellationRequested) { }
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
