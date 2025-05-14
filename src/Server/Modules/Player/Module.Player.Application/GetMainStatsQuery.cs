using System.Threading.Channels;
using Server.Module.Player.Domain;
using Server.Module.Player.Infrastructure;
using Server.Shared.Cqrs;
using Server.Shared.Results;

namespace Server.Module.Player.Application;

public sealed record GetMainStatsQuery(Guid MainStatsId) : IQuery<MainStats>;

public sealed class GetMainStatsQueryHandler(IMainStatsRepository mainStatsRepository)
    : IQueryHandler<GetMainStatsQuery, MainStats>
{
    /// <summary>
    /// Handles a query to retrieve a <c>MainStats</c> entity by its unique identifier.
    /// </summary>
    /// <param name="playerQuery">The query containing the ID of the main stats to retrieve.</param>
    /// <param name="token">Cancellation token for the asynchronous operation.</param>
    /// <returns>
    /// A result containing the <c>MainStats</c> entity if found; otherwise, a failure result with a not found error.
    /// </returns>
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
}

public sealed record SubscribeMainStats(Guid MainStatsId) : IQuery<IAsyncEnumerable<MainStats>>;

public sealed class SubscribeMainStatsHandler(
    IMainStatsRepository mainStatsRepository,
    IMainStatsChangeNotifier notifier
) : IQueryHandler<SubscribeMainStats, IAsyncEnumerable<MainStats>>
{
    /// <summary>
    /// Handles a subscription query for a MainStats entity, returning an asynchronous stream of updates for the specified MainStatsId.
    /// </summary>
    /// <param name="query">The subscription query containing the MainStatsId to monitor.</param>
    /// <param name="cancellationToken">Token to cancel the subscription and release resources.</param>
    /// <returns>
    /// A result containing an asynchronous enumerable that yields the current and subsequent updates of the MainStats entity,
    /// or a failure result if the entity is not found.
    /// </returns>
    public async Task<Result<IAsyncEnumerable<MainStats>>> Handle(
        SubscribeMainStats query,
        CancellationToken cancellationToken = default
    )
    {
        MainStats? mainStats = await mainStatsRepository.GetAsync(
            query.MainStatsId,
            cancellationToken
        );
        if (mainStats is null)
        {
            return Result.Failure<IAsyncEnumerable<MainStats>>(
                MainStatsError.NotFound(query.MainStatsId)
            );
        }

        // Создаем канал для передачи обновлений
        Channel<MainStats> channel = Channel.CreateUnbounded<MainStats>();

        // Сразу отправляем текущее состояние
        await channel.Writer.WriteAsync(mainStats, cancellationToken);

        // Регистрируем обработчик изменений и передаем их в канал
        IDisposable subscription = notifier.Subscribe(
            query.MainStatsId,
            async (updatedStats) =>
            {
                await channel.Writer.WriteAsync(updatedStats, cancellationToken);
            }
        );

        // Обрабатываем отмену для очистки ресурсов
        cancellationToken.Register(() =>
        {
            subscription.Dispose();
            channel.Writer.Complete();
        });

        // Возвращаем асинхронный поток данных
        return Result.Success(channel.Reader.ReadAllAsync(cancellationToken));
    }
}
