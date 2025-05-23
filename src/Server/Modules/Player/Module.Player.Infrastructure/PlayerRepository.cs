using Server.Module.Player.Application;
using Server.Module.Player.Infrastructure.EfCore;
using Server.Shared.Errors;

namespace Server.Module.Player.Infrastructure;

public class PlayerRepository(PlayerEventListener eventListener, Context context) : IPlayerRepository
{
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

    public async Task SaveAsync(Domain.Player model, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(model.ToPlayerEntity());
        await context.SaveChangesAsync();

        // Убеждаемся, что сущность отслеживается
        eventListener.TrackEntity(model);

        return;
    }
}
