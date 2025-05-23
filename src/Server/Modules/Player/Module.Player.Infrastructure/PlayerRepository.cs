using Server.Module.Player.Application;
using Server.Module.Player.Infrastructure.EfCore;
using Server.Shared.Errors;

namespace Server.Module.Player.Infrastructure;

public class PlayerRepository(PlayerEventListener eventListener, Context context) : IPlayerRepository
{
    /// <summary>
    /// Asynchronously retrieves a player by ID from the data store, returning the domain player if found or null if not.
    /// </summary>
    /// <param name="id">The unique identifier of the player to retrieve.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The domain player if found; otherwise, null.</returns>
    public async Task<Domain.Player?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Получаем сущность из хранилища
        Result<PlayerEntity>? result = await context.PlayerEntities.FindAsync(id, cancellationToken);

        if (result.IsFailure)
            return null;

        Domain.Player? player = result.Value.ToPlayerDomain();

        if (player != null)
        {
            // Начинаем отслеживать изменения в загруженной сущности
            eventListener.TrackEntity(player);
        }

        return player;
    }

    /// <summary>
    /// Persists a new player to the data store and registers it for event tracking.
    /// </summary>
    /// <param name="model">The domain player to be saved.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    public async Task SaveAsync(Domain.Player model, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(model.ToPlayerEntity());
        await context.SaveChangesAsync();

        // Убеждаемся, что сущность отслеживается
        eventListener.TrackEntity(model);

        return;
    }
}
