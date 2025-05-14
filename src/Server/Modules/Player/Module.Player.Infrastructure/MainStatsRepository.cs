using Server.Module.Player.Domain;
using Server.Shared.Results;

namespace Server.Module.Player.Infrastructure;

public class MainStatsRepository(MainStatsEventListener eventListener) : IMainStatsRepository
{
    public Task<MainStats?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Получаем сущность из хранилища
        Result<MainStats>? result = MainStats.CreatePlayer($"Player_{id}");

        if (result.IsFailure)
            return Task.FromResult<MainStats>(null);

        MainStats? entity = result.Value;

        if (entity != null)
        {
            // Начинаем отслеживать изменения в загруженной сущности
            eventListener.TrackEntity(result.Value);
        }

        return Task.FromResult(entity);
    }

    public Task SaveAsync(MainStats entity, CancellationToken cancellationToken = default)
    {
        // Сохраняем сущность в хранилище
        // ...

        // Убеждаемся, что сущность отслеживается
        eventListener.TrackEntity(entity);

        return Task.CompletedTask;
    }
}
