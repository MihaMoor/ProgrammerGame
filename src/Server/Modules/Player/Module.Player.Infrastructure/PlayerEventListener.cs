using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using System.Collections.Concurrent;

namespace Server.Module.Player.Infrastructure;

public class PlayerEventListener(IPlayerChangeNotifier _notifier, ILogger<PlayerEventListener> Logger)
{
    private readonly ConcurrentDictionary<Guid, Domain.Player> _trackedEntities = new();

    /// <summary>
    /// Begins tracking the specified player entity and subscribes to its stats change events.
    /// </summary>
    /// <param name="entity">The player entity to track.</param>
    public void TrackEntity(Domain.Player entity)
    {
        if (_trackedEntities.TryAdd(entity.PlayerId, entity))
        {
            // Подписываемся на события сущности
            entity.StatsChanged += OnEntityChanged;
        }
    }

    /// <summary>
    /// Stops tracking the specified player entity and unsubscribes from its stats change notifications.
    /// </summary>
    /// <param name="entity">The player entity to untrack.</param>
    public void UntrackEntity(Domain.Player entity)
    {
        if (_trackedEntities.TryRemove(entity.PlayerId, out _))
        {
            entity.StatsChanged -= OnEntityChanged;
        }
    }

    /// <summary>
    /// Handles the player's stats change event by asynchronously notifying subscribers of the update.
    /// </summary>
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
