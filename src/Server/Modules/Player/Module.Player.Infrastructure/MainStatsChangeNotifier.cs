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

    // Вызывается при изменении MainStats
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

    // Метод для отписки
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
