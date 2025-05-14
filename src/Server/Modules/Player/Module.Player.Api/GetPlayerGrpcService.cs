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
    /// <summary>
    /// Retrieves the main player statistics for the specified UUID and returns them as a PlayerDto.
    /// </summary>
    /// <param name="request">The UUID of the player whose statistics are requested.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A PlayerDto containing the player's main statistics, or an empty PlayerDto if retrieval fails.</returns>
    public override async Task<PlayerDto> Get(UUID request, ServerCallContext context)
    {
        GetMainStatsQuery query = new(new(request.Id));

        Result<MainStats> result = await handler.Handle(query);

        if (result.IsFailure)
        {
            logger.LogError(result.Error.ToString());
            return new PlayerDto();
        }

        return result.Value.ToViewModel();
    }
}

internal static class PlayerExtensions
{
    /// <summary>
        /// Converts a <see cref="MainStats"/> domain object to a <see cref="PlayerDto"/> view model.
        /// </summary>
        /// <param name="stats">The player's main statistics to convert.</param>
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
