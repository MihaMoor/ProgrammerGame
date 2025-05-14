using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public class MainStatsEventListener(MainStatsChangeNotifier _notifier, ILogger Logger)
{
    private readonly ConcurrentDictionary<Guid, MainStats> _trackedEntities = new();

    /// <summary>
    /// Begins tracking the specified <see cref="MainStats"/> entity and subscribes to its stats change events.
    /// </summary>
    /// <param name="entity">The <see cref="MainStats"/> entity to track.</param>
    public void TrackEntity(MainStats entity)
    {
        if (_trackedEntities.TryAdd(entity.MainStatsId, entity))
        {
            // Подписываемся на события сущности
            entity.StatsChanged += OnEntityChanged;
        }
    }

    /// <summary>
    /// Stops tracking the specified MainStats entity and unsubscribes from its StatsChanged event.
    /// </summary>
    /// <param name="entity">The MainStats entity to untrack.</param>
    public void UntrackEntity(MainStats entity)
    {
        if (_trackedEntities.TryRemove(entity.MainStatsId, out _))
        {
            entity.StatsChanged -= OnEntityChanged;
        }
    }

    /// <summary>
    /// Handles a stats change event for a tracked MainStats entity by asynchronously notifying subscribers.
    /// </summary>
    private void OnEntityChanged(MainStats entity)
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
