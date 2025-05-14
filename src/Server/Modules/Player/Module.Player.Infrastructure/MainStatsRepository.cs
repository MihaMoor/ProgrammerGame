using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public class MainStatsRepository(MainStatsEventListener eventListener) : IMainStatsRepository
{
    public async Task<MainStats?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Получаем сущность из хранилища
        MainStats entity = MainStats.CreatePlayer("From repository").Value;

        if (entity != null)
        {
            // Начинаем отслеживать изменения в загруженной сущности
            eventListener.TrackEntity(entity);
        }

        return entity;
    }

    public async Task SaveAsync(MainStats entity, CancellationToken cancellationToken = default)
    {
        // Сохраняем сущность в хранилище
        // ...

        // Убеждаемся, что сущность отслеживается
        eventListener.TrackEntity(entity);
    }
}
