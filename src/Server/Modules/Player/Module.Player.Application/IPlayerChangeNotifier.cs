namespace Server.Module.Player.Application;

public interface IPlayerChangeNotifier
{
    /// <summary>
/// Subscribes to changes for the specified player and registers a handler to be invoked when updates occur.
/// </summary>
/// <param name="playerId">The unique identifier of the player to observe.</param>
/// <param name="handler">An asynchronous function to handle player updates.</param>
/// <returns>An <see cref="IDisposable"/> that can be used to cancel the subscription.</returns>
    IDisposable Subscribe(Guid playerId, Func<Domain.Player, Task> handler);
    /// <summary>
/// Notifies the system that the main statistics of the specified player have changed.
/// </summary>
/// <param name="player">The player whose main statistics have been updated.</param>
/// <returns>A task representing the asynchronous notification operation.</returns>
Task OnMainStatsChanged(Domain.Player player);
}
