using Server.Module.Player.Domain;
using Server.Shared.Results;

namespace Server.Module.Player.Infrastructure;

public class MainStatsRepository(MainStatsEventListener eventListener) : IMainStatsRepository
{
    /// <summary>
    /// Retrieves a <see cref="MainStats"/> entity for the specified player ID, or null if creation fails.
    /// </summary>
    /// <param name="id">The unique identifier of the player.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The <see cref="MainStats"/> entity if successful; otherwise, null.</returns>
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

    /// <summary>
    /// Saves the specified MainStats entity and ensures it is tracked for changes.
    /// </summary>
    /// <param name="entity">The MainStats entity to save and track.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A completed task representing the save operation.</returns>
    public Task SaveAsync(MainStats entity, CancellationToken cancellationToken = default)
    {
        // Сохраняем сущность в хранилище
        // ...

        // Убеждаемся, что сущность отслеживается
        eventListener.TrackEntity(entity);

        return Task.CompletedTask;
    }
}
