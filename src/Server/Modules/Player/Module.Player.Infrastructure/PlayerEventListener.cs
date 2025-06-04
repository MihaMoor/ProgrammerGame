using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using System.Collections.Concurrent;

namespace Server.Module.Player.Infrastructure;

public class PlayerEventListener(IPlayerChangeNotifier _notifier, ILogger<PlayerEventListener> Logger)
{
    private readonly ConcurrentDictionary<Guid, Domain.Player> _trackedEntities = new();

    /// <summary>
    /// Начинает отслеживать указанный объект игрока и подписывается на события изменения его характеристик.
    /// </summary>
    /// <param name="entity">Объект игрока для отслеживания.</param>
    public void TrackEntity(Domain.Player entity)
    {
        if (_trackedEntities.TryAdd(entity.PlayerId, entity))
        {
            // Подписываемся на события сущности
            entity.StatsChanged += OnEntityChanged;
        }
    }

    /// <summary>
    /// Прекращает отслеживание указанного объекта игрока и отписывается от его события изменения характеристик (StatsChanged).
    /// </summary>
    /// <param name="entity">Объект игрока, которого необходимо перестать отслеживать.</param>
    public void UntrackEntity(Domain.Player entity)
    {
        if (_trackedEntities.TryRemove(entity.PlayerId, out _))
        {
            entity.StatsChanged -= OnEntityChanged;
        }
    }

    /// <summary>
    /// Обрабатывает событие изменения характеристик игрока, асинхронно уведомляя подписчиков об обновлении.
    /// </summary>
    /// <param name="entity">Объект игрока, у которого изменились характеристики.</param>
    private void OnEntityChanged(Domain.Player entity)
    {
        // Асинхронно уведомляем всех подписчиков
        Task.Run(async () =>
        {
            try
            {
                await _notifier.OnMainStatsChanged(entity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
        });
    }
}
