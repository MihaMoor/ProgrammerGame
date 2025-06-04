using System.Threading.Channels;
using Server.Module.Player.Domain;
using Server.Shared.Cqrs;
using Server.Shared.Errors;

namespace Server.Module.Player.Application;

public sealed record GetPlayerQuery(Guid PlayerId) : IQuery<Domain.Player>;

public sealed class GetPlayerQueryHandler(IPlayerRepository playerRepository)
    : IQueryHandler<GetPlayerQuery, Domain.Player>
{
    /// <summary>
    /// Handles a query to retrieve a player by their unique identifier.
    /// </summary>
    /// <param name="playerQuery">The query containing the player ID to retrieve.</param>
    /// <param name="token">A cancellation token for the operation.</param>
    /// <returns>A result containing the player if found; otherwise, a failure result indicating the player was not found.</returns>
    public async Task<Result<Domain.Player>> Handle(
        GetPlayerQuery playerQuery,
        CancellationToken token
    )
    {
        Domain.Player? player = await playerRepository.GetAsync(playerQuery.PlayerId, token);
        if (player is null)
        {
            return Result.Failure<Domain.Player>(PlayerError.NotFound(playerQuery.PlayerId));
        }
        return Result.Success(player);
    }
}

public sealed record SubscribePlayer(Guid PlayerId) : IQuery<IAsyncEnumerable<Domain.Player>>;

public sealed class SubscribePlayerHandler(
    IPlayerRepository playerRepository,
    IPlayerChangeNotifier notifier
) : IQueryHandler<SubscribePlayer, IAsyncEnumerable<Domain.Player>>
{
    /// <summary>
    /// Handles a subscription request for real-time updates to a player's state.
    /// </summary>
    /// <param name="query">The subscription query specifying the player to observe.</param>
    /// <param name="cancellationToken">Token to signal cancellation of the subscription.</param>
    /// <returns>
    /// A result containing an asynchronous stream of player updates if the player exists; otherwise, a failure result indicating the player was not found.
    /// </returns>
    public async Task<Result<IAsyncEnumerable<Domain.Player>>> Handle(
        SubscribePlayer query,
        CancellationToken cancellationToken = default
    )
    {
        Domain.Player? player = await playerRepository.GetAsync(
            query.PlayerId,
            cancellationToken
        );
        if (player is null)
        {
            return Result.Failure<IAsyncEnumerable<Domain.Player>>(
                PlayerError.NotFound(query.PlayerId)
            );
        }

        // Создаем канал для передачи обновлений
        Channel<Domain.Player> channel = Channel.CreateBounded<Domain.Player>(
            new BoundedChannelOptions(10) // ёмкость подберите опытным путём
            {
                FullMode = BoundedChannelFullMode.DropOldest,
            }
        );

        // Сразу отправляем текущее состояние
        await channel.Writer.WriteAsync(player, cancellationToken);

        // Регистрируем обработчик изменений и передаем их в канал
        IDisposable subscription = notifier.Subscribe(
            query.PlayerId,
            updatedStats =>
            {
                channel.Writer.TryWrite(updatedStats);
                return Task.CompletedTask;
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
