using Server.Module.Player.Application;
using Server.Module.Player.Infrastructure.EfCore;
using Server.Shared.Errors;

namespace Server.Module.Player.Infrastructure;

public class PlayerRepository(PlayerEventListener eventListener, Context context) : IPlayerRepository
{
    /// <summary>
    /// Asynchronously retrieves a player domain model by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the player.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The player domain model if found; otherwise, null.</returns>
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
    /// Saves or updates a player domain model in the database asynchronously.
    /// </summary>
    /// <param name="model">The player domain model to be saved or updated.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
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
