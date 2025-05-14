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
    /// <summary>
    /// Handles a gRPC request to retrieve a player's main statistics by UUID and streams the result to the client.
    /// </summary>
    /// <param name="request">The UUID of the player whose data is requested.</param>
    /// <param name="responseStream">The stream used to send the <see cref="PlayerDto"/> response.</param>
    /// <param name="context">The context for the gRPC call, used for cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
    /// <summary>
        /// Converts a <see cref="MainStats"/> instance to a <see cref="PlayerDto"/> by mapping its properties.
        /// </summary>
        /// <param name="stats">The main stats to convert.</param>
        /// <returns>A <see cref="PlayerDto"/> populated with values from <paramref name="stats"/>.</returns>
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
