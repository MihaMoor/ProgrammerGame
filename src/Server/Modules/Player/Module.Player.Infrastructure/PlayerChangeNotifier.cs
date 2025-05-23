using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using System.Collections.Concurrent;

namespace Server.Module.Player.Infrastructure;

public class PlayerChangeNotifier(ILogger _logger) : IPlayerChangeNotifier
{
    private readonly ConcurrentDictionary<
        Guid,
        ConcurrentDictionary<Guid, Func<Domain.Player, Task>>
    > _subscriptions = new();

    /// <summary>
    /// Subscribes an asynchronous handler to be notified when the specified player's data changes.
    /// </summary>
    /// <param name="mainStatsId">The unique identifier of the player to subscribe to.</param>
    /// <param name="handler">The asynchronous handler to invoke when the player's data changes.</param>
    /// <returns>An <see cref="IDisposable"/> that can be used to unsubscribe the handler.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="handler"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a subscription ID collision occurs (extremely rare).</exception>
    public IDisposable Subscribe(Guid mainStatsId, Func<Domain.Player, Task> handler)
    {
        // Создаем уникальный ID для подписки
        Guid subscriptionId = Guid.NewGuid();

        // Получаем или создаем внутренний словарь для указанного mainStatsId
        ConcurrentDictionary<Guid, Func<Domain.Player, Task>> handlersDict = _subscriptions.GetOrAdd(
            mainStatsId,
            _ => new ConcurrentDictionary<Guid, Func<Domain.Player, Task>>()
        );

        ArgumentNullException.ThrowIfNull(handler);

        // Добавляем обработчик во внутренний словарь
        if (!handlersDict.TryAdd(subscriptionId, handler))
        {
            throw new InvalidOperationException($"Subscription id collision: {subscriptionId}");
        }

        // Возвращаем объект подписки
        return new Subscription(mainStatsId, subscriptionId, this);
    }

    /// <summary>
    /// Notifies all subscribed handlers of a change to the specified player's stats by invoking them asynchronously in parallel.
    /// </summary>
    /// <param name="stats">The player whose stats have changed.</param>
    internal async Task OnMainStatsChanged(Domain.Player stats)
    {
        if (
            _subscriptions.TryGetValue(
                stats.PlayerId,
                out ConcurrentDictionary<Guid, Func<Domain.Player, Task>>? handlersDict
            )
        )
        {
            // Запускаем все обработчики параллельно
            IEnumerable<Task> tasks = handlersDict.Values.Select(handler =>
                SafeInvokeAsync(handler, stats, _logger)
            );

            await Task.WhenAll(tasks);
        }
    }

    /// <summary>
    /// Invokes the specified player event handler asynchronously, logging any exceptions without propagating them.
    /// </summary>
    private static async Task SafeInvokeAsync(
        Func<Domain.Player, Task> handler,
        Domain.Player stats,
        ILogger logger
    )
    {
        try
        {
            await handler(stats);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обработке события изменения Player");
        }
    }

    /// <summary>
    /// Removes a subscription handler for the specified player and subscription IDs.
    /// </summary>
    /// <param name="mainStatsId">The unique identifier of the player whose subscription is to be removed.</param>
    /// <param name="subscriptionId">The unique identifier of the subscription to remove.</param>
    internal void Unsubscribe(Guid mainStatsId, Guid subscriptionId)
    {
        // Если для mainStatsId есть словарь обработчиков
        if (
            _subscriptions.TryGetValue(
                mainStatsId,
                out ConcurrentDictionary<Guid, Func<Domain.Player, Task>>? handlersDict
            )
        )
        {
            // Удаляем обработчик по его ID
            handlersDict.TryRemove(subscriptionId, out _);

            // Если словарь обработчиков пуст, удаляем его из основного словаря
            if (handlersDict.IsEmpty)
            {
                _subscriptions.TryRemove(mainStatsId, out _);
            }
        }
    }

    // Внутренний класс для представления подписки
    private class Subscription(
        Guid mainStatsId,
        Guid subscriptionId,
        PlayerChangeNotifier notifier
    ) : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Unsubscribes from player change notifications and releases the subscription.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                notifier.Unsubscribe(mainStatsId, subscriptionId);
                _disposed = true;
            }
        }
    }
}
