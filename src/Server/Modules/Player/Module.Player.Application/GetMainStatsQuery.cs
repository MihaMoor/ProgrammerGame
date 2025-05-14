using Server.Module.Player.Domain;

namespace Server.Module.Player.Application;

public sealed record GetMainStatsQuery(Guid PlayerId) : IQuery<MainStats>;

public sealed class GetMainStatsQueryHandler(IMainStatsRepository mainStatsRepository)
    : IQueryHandler<GetMainStatsQuery, MainStats>
{
    public async Task<Result<MainStats>> Handle(
        GetMainStatsQuery playerQuery,
        CancellationToken token
    )
    {
        MainStats? mainStats = await mainStatsRepository.GetAsync(playerQuery.PlayerId, token);
        if (mainStats is null)
        {
            return Result.Failure<MainStats>(MainStatsError.NotFound(playerQuery.PlayerId));
        }
        return Result.Success(mainStats);
    }
}
