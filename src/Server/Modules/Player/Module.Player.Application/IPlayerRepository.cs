namespace Server.Module.Player.Application;

public interface IPlayerRepository
{
    /// <summary>
    /// Получение игрока по Id.
    /// </summary>
    /// <param name="id">Id игрока</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <summary>
/// Asynchronously retrieves a player by their unique identifier.
/// </summary>
/// <param name="id">The unique identifier of the player to retrieve.</param>
/// <param name="cancellationToken">A token to cancel the operation.</param>
/// <returns>The player with the specified ID, or null if not found.</returns>
    Task<Domain.Player?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
