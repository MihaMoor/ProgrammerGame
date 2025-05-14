using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public class MainStatsEventListener(MainStatsChangeNotifier _notifier, ILogger Logger)
{
    private readonly ConcurrentDictionary<Guid, MainStats> _trackedEntities = new();

    // Этот метод вызывается, когда MainStats загружается из репозитория
    public void TrackEntity(MainStats entity)
    {
        if (_trackedEntities.TryAdd(entity.MainStatsId, entity))
        {
            // Подписываемся на события сущности
            entity.StatsChanged += OnEntityChanged;
        }
    }

    // Прекращаем отслеживание сущности
    public void UntrackEntity(MainStats entity)
    {
        if (_trackedEntities.TryRemove(entity.MainStatsId, out _))
        {
            entity.StatsChanged -= OnEntityChanged;
        }
    }

    private void OnEntityChanged(MainStats entity)
    {
        // Асинхронно уведомляем всех подписчиков
        _ = _notifier.OnMainStatsChanged(entity);
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
