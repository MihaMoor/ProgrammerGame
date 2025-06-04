using Server.Module.Player.Application;
using Server.Module.Player.Infrastructure.EfCore;
using Server.Shared.Errors;

namespace Server.Module.Player.Infrastructure;

public class PlayerRepository(PlayerEventListener eventListener, Context context) : IPlayerRepository
{
    /// <summary>
    /// Асинхронно получает модель домена игрока по его уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор игрока.</param>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции.</param>
    /// <returns>Модель домена игрока, если она найдена; иначе null.</returns>
    public async Task<Domain.Player?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Получаем сущность из хранилища
        Result<PlayerEntity>? result = await context.PlayerEntities.FindAsync(
            [id, cancellationToken],
            cancellationToken: cancellationToken);

        if (result.IsFailure)
            return null;

        var player = result.Value.ToPlayerDomain();

        if (player != null)
        {
            // Начинаем отслеживать изменения в загруженной сущности
            eventListener.TrackEntity(player);
        }

        return player;
    }

    /// <summary>
    /// Асинхронно сохраняет или обновляет модель домена игрока в базе данных.
    /// </summary>
    /// <param name="model">Модель домена игрока, которую необходимо сохранить или обновить.</param>
    /// <param name="cancellationToken">Токен для отслеживания запросов отмены.</param>
    public async Task SaveAsync(Domain.Player model, CancellationToken cancellationToken = default)
    {
        PlayerEntity playerEntity = model.ToPlayerEntity();
        var existingEntity = await context.PlayerEntities.FindAsync([model.PlayerId], cancellationToken);

        if(existingEntity is null)
        {
            await context.PlayerEntities.AddAsync(playerEntity,cancellationToken);
        }
        else
        {
            context.Entry(existingEntity).CurrentValues.SetValues(playerEntity);
        }

        await context.SaveChangesAsync(cancellationToken);

        // Убеждаемся, что сущность отслеживается
        eventListener.TrackEntity(model);

        return;
    }
}
