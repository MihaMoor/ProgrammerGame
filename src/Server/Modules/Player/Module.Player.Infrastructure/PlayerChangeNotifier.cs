using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using System.Collections.Concurrent;

namespace Server.Module.Player.Infrastructure;

public class PlayerChangeNotifier(ILogger<PlayerChangeNotifier> _logger) : IPlayerChangeNotifier
{
    private readonly ConcurrentDictionary<
        Guid,
        ConcurrentDictionary<Guid, Func<Domain.Player, Task>>
    > _subscriptions = new();

    /// <summary>
    /// Подписывает асинхронный обработчик на получение уведомлений об изменении состояния указанного игрока.
    /// </summary>
    /// <param name="playerId">Уникальный идентификатор игрока для подписки.</param>
    /// <param name="handler">Асинхронный обработчик, вызываемый при изменении состояния игрока.</param>
    /// <returns><see cref="IDisposable"/>, который может быть использован для отписки обработчика.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="handler"/> равен null.</exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается при коллизии идентификаторов подписки (крайне редко).
    /// </exception>
    public IDisposable Subscribe(Guid playerId, Func<Domain.Player, Task> handler)
    {
        // Создаем уникальный ID для подписки
        Guid subscriptionId = Guid.NewGuid();

        // Получаем или создаем внутренний словарь для указанного playerId
        ConcurrentDictionary<Guid, Func<Domain.Player, Task>> handlersDict = _subscriptions.GetOrAdd(
            playerId,
            _ => new ConcurrentDictionary<Guid, Func<Domain.Player, Task>>()
        );

        ArgumentNullException.ThrowIfNull(handler);

        // Добавляем обработчик во внутренний словарь
        if (!handlersDict.TryAdd(subscriptionId, handler))
        {
            throw new InvalidOperationException($"Subscription id collision: {subscriptionId}");
        }

        // Возвращаем объект подписки
        return new Subscription(playerId, subscriptionId, this);
    }

    /// <summary>
    /// Уведомляет все подписанные обработчики об изменении основных характеристик указанного игрока.
    /// </summary>
    /// <param name="stats">Игрок, основные характеристики которого изменились.</param>
    /// <remarks>
    /// Вызывает все зарегистрированные обработчики для идентификатора игрока параллельно.
    /// Если обработчики не зарегистрированы, никаких действий не предпринимается.
    /// </remarks>
    public async Task OnMainStatsChanged(Domain.Player stats)
    {
        ArgumentNullException.ThrowIfNull(stats);

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
    /// Асинхронно вызывает указанный обработчик с переданными характеристиками игрока, регистрируя любые возникающие исключения.
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
    /// Удаляет обработчик подписки для указанного игрока и идентификаторов подписки.
    /// </summary>
    /// <param name="mainStatsId">Уникальный идентификатор игрока.</param>
    /// <param name="subscriptionId">Уникальный идентификатор подписки для удаления.</param>
    internal void Unsubscribe(Guid mainStatsId, Guid subscriptionId)
    {
        // Если для playerId есть словарь обработчиков
        if (
            _subscriptions.TryGetValue(
                mainStatsId,
                out ConcurrentDictionary<Guid, Func<Domain.Player, Task>>? handlersDict
            )
        )
        {
            // Удаляем обработчик по его ID
            handlersDict.TryRemove(subscriptionId, out _);

            // Пытаемся удалить только если словарь действительно пуст
            if (handlersDict.IsEmpty)
            {
                _subscriptions.TryRemove(mainStatsId, out var removedDict);
                // Если удаленный словарь не пуст, добавляем его обратно
                if (removedDict != null && !removedDict.IsEmpty)
                {
                    _subscriptions.TryAdd(mainStatsId, removedDict);
                }
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
        /// Отписывается от уведомлений об изменениях игрока и освобождает подписку.
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
