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
