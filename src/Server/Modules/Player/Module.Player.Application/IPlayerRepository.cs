namespace Server.Module.Player.Application;

public interface IPlayerRepository
{
    /// <summary>
    /// Получение игрока по Id.
    /// </summary>
    /// <param name="id">Id игрока</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <summary>
/// Asynchronously retrieves a player entity by its unique identifier.
/// </summary>
/// <param name="id">The unique identifier of the player to retrieve.</param>
/// <param name="cancellationToken">Optional token to cancel the operation.</param>
/// <returns>The player entity if found; otherwise, null.</returns>
    Task<Domain.Player?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
