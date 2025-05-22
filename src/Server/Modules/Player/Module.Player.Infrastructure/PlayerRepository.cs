using Server.Module.Player.Application;
using Server.Shared.Errors;

namespace Server.Module.Player.Infrastructure;

public class PlayerRepository(PlayerEventListener eventListener, IPostgreSqlContext postgreSqlContext) : IPlayerRepository
{
    public async Task<Domain.Player?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Получаем сущность из хранилища
        Result<Domain.Player>? result = await postgreSqlContext.GetAsync(id, cancellationToken);

        if (result.IsFailure)
            return null;

        Domain.Player? entity = result.Value;

        if (entity != null)
        {
            // Начинаем отслеживать изменения в загруженной сущности
            eventListener.TrackEntity(entity);
        }

        return entity;
    }

    public Task SaveAsync(Domain.Player entity, CancellationToken cancellationToken = default)
    {
        // Сохраняем сущность в хранилище
        // ...

        // Убеждаемся, что сущность отслеживается
        eventListener.TrackEntity(entity);

        return Task.CompletedTask;
    }
}

public interface IPostgreSqlContext
{
    Task<Domain.Player> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
