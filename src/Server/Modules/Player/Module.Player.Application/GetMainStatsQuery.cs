using Server.Module.Player.Domain;
using Server.Shared.Cqrs;
using Server.Shared.Results;

namespace Server.Module.Player.Application;

public sealed record GetMainStatsQuery(Guid MainStatsId) : IQuery<MainStats>;

public sealed class GetMainStatsQueryHandler(IMainStatsRepository mainStatsRepository)
    : IQueryHandler<GetMainStatsQuery, MainStats>
{
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
