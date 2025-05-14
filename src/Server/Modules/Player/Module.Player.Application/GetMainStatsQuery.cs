using Server.Module.Player.Domain;
using Server.Shared.Cqrs;
using Server.Shared.Results;

namespace Server.Module.Player.Application;

public sealed record GetMainStatsQuery(Guid MainStatsId) : IQuery<MainStats>;

public sealed class GetMainStatsQueryHandler(IMainStatsRepository mainStatsRepository)
    : IQueryHandler<GetMainStatsQuery, MainStats>
{
    /// <summary>
    /// Retrieves the main statistics for a player by ID.
    /// </summary>
    /// <param name="playerQuery">The query containing the main stats identifier.</param>
    /// <param name="token">Cancellation token for the asynchronous operation.</param>
    /// <returns>A result containing the main stats if found; otherwise, a failure result indicating not found.</returns>
    public async Task<Result<MainStats>> Handle(
        GetMainStatsQuery playerQuery,
        CancellationToken token
    )
    {
        MainStats? mainStats = await mainStatsRepository.GetAsync(playerQuery.MainStatsId, token);
        if (mainStats is null)
        {
            return Result.Failure<MainStats>(MainStatsError.NotFound(playerQuery.MainStatsId));
        }
        return Result.Success(mainStats);
    }

    /// <summary>
    /// Explicit interface implementation for handling a <see cref="GetMainStatsQuery"/>; not implemented.
    /// </summary>
    /// <param name="query">The query containing the main stats identifier.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>Throws <see cref="NotImplementedException"/>.</returns>
    Task<Result<MainStats>> IQueryHandler<GetMainStatsQuery, MainStats>.Handle(
        GetMainStatsQuery query,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}

public sealed class SubscribeMainStats(Guid MainStatsId) : IQuery<MainStats>
{
    // Тут должна быть логика подписки на слой Application и получения от него данных
}
