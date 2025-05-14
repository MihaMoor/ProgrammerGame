using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public class MainStatsChangeNotifier(ILogger _logger) : IMainStatsChangeNotifier
{
    private readonly ConcurrentDictionary<
        Guid,
        ConcurrentBag<(Func<MainStats, Task> Handler, IDisposable Subscription)>
    > _subscriptions = new();

    /// <summary>
    /// Subscribes an asynchronous handler to be notified when the specified MainStats entity changes.
    /// </summary>
    /// <param name="mainStatsId">The unique identifier of the MainStats entity to observe.</param>
    /// <param name="handler">An asynchronous delegate to invoke when the MainStats changes.</param>
    /// <returns>An IDisposable that can be used to unsubscribe the handler.</returns>
    public IDisposable Subscribe(Guid mainStatsId, Func<MainStats, Task> handler)
    {
        Subscription subscription = new(mainStatsId, handler, this);

        // Добавляем подписку в словарь
        _subscriptions.AddOrUpdate(
            mainStatsId,
            // Если записи для этого ID нет, создаем новую коллекцию
            _ => new ConcurrentBag<(Func<MainStats, Task>, IDisposable)>
            {
                (handler, subscription),
            },
            // Если запись существует, добавляем в существующую коллекцию
            (_, existing) =>
            {
                existing.Add((handler, subscription));
                return existing;
            }
        );

        return subscription;
    }

    /// <summary>
    /// Asynchronously notifies all subscribed handlers of changes to the specified <see cref="MainStats"/> instance.
    /// </summary>
    /// <param name="stats">The updated <see cref="MainStats"/> object whose subscribers should be notified.</param>
    /// <remarks>
    /// Invokes all registered handlers for the given <c>MainStatsId</c> in parallel. Exceptions thrown by handlers are caught and logged without interrupting other notifications.
    /// </remarks>
    internal async Task OnMainStatsChanged(MainStats stats)
    {
        if (
            _subscriptions.TryGetValue(
                stats.MainStatsId,
                out ConcurrentBag<(
                    Func<MainStats, Task> Handler,
                    IDisposable Subscription
                )>? handlers
            )
        )
        {
            IEnumerable<Task> tasks = handlers.Select(h =>
                SafeInvokeAsync(h.Handler, stats, _logger)
            );

            await Task.WhenAll(tasks);

            static async Task SafeInvokeAsync(Func<MainStats, Task> h, MainStats s, ILogger _logger)
            {
                try
                {
                    await h(s);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            }
        }
    }

    /// <summary>
    /// Removes a specific subscription handler for the given MainStatsId.
    /// </summary>
    /// <param name="mainStatsId">The identifier of the MainStats entity.</param>
    /// <param name="subscription">The subscription to remove.</param>
    internal void Unsubscribe(Guid mainStatsId, IDisposable subscription)
    {
        if (
            _subscriptions.TryGetValue(
                mainStatsId,
                out ConcurrentBag<(
                    Func<MainStats, Task> Handler,
                    IDisposable Subscription
                )>? handlers
            )
        )
        {
            ConcurrentBag<(Func<MainStats, Task>, IDisposable)> updated =
            [
                .. handlers.Where(x => x.Subscription != subscription),
            ];

            if (updated.IsEmpty)
            {
                _subscriptions.TryRemove(mainStatsId, out _);
            }
            else
            {
                _subscriptions[mainStatsId] = updated;
            }
        }
    }

    // Внутренний класс для представления подписки
    private class Subscription : IDisposable
    {
        private readonly Guid _mainStatsId;
        private readonly Func<MainStats, Task> _handler;
        private readonly MainStatsChangeNotifier _notifier;
        private bool _disposed;

        /// <summary>
        /// Initializes a new subscription for a specific MainStatsId and handler within the MainStatsChangeNotifier.
        /// </summary>
        /// <param name="mainStatsId">The identifier of the MainStats entity to subscribe to.</param>
        /// <param name="handler">The asynchronous handler to invoke when the MainStats changes.</param>
        /// <param name="notifier">The parent notifier managing this subscription.</param>
        public Subscription(
            Guid mainStatsId,
            Func<MainStats, Task> handler,
            MainStatsChangeNotifier notifier
        )
        {
            _mainStatsId = mainStatsId;
            _handler = handler;
            _notifier = notifier;
        }

        /// <summary>
        /// Unsubscribes from notifications for the associated MainStatsId and releases resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _notifier.Unsubscribe(_mainStatsId, this);
                _disposed = true;
            }
        }
    }
}
