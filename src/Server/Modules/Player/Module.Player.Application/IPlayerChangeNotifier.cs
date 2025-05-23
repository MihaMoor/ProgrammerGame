namespace Server.Module.Player.Application;

public interface IPlayerChangeNotifier
{
    /// <summary>
/// Subscribes to changes of a <see cref="Domain.Player"/> identified by the specified main statistics ID.
/// </summary>
/// <param name="mainStatsId">The unique identifier of the player's main statistics.</param>
/// <param name="handler">An asynchronous handler invoked when the player's data changes.</param>
/// <returns>An <see cref="IDisposable"/> that can be used to cancel the subscription.</returns>
    IDisposable Subscribe(Guid mainStatsId, Func<Domain.Player, Task> handler);
}
