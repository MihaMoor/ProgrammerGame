using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public class MainStatsEventListener(MainStatsChangeNotifier _notifier, ILogger Logger)
{
    private readonly ConcurrentDictionary<Guid, Domain.Player> _trackedEntities = new();

    // Этот метод вызывается, когда Player загружается из репозитория
    public void TrackEntity(Domain.Player entity)
    {
        if (_trackedEntities.TryAdd(entity.PlayerId, entity))
        {
            // Подписываемся на события сущности
            entity.StatsChanged += OnEntityChanged;
        }
    }

    // Прекращаем отслеживание сущности
    public void UntrackEntity(Domain.Player entity)
    {
        if (_trackedEntities.TryRemove(entity.PlayerId, out _))
        {
            entity.StatsChanged -= OnEntityChanged;
        }
    }

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
