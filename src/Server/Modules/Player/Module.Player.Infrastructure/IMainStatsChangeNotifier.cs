using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public interface IMainStatsChangeNotifier
{
    /// <summary>
/// Subscribes to changes in <see cref="MainStats"/> for the specified ID.
/// </summary>
/// <param name="mainStatsId">The unique identifier of the <see cref="MainStats"/> to observe.</param>
/// <param name="handler">An asynchronous callback invoked when the <see cref="MainStats"/> changes.</param>
/// <returns>An <see cref="IDisposable"/> that can be used to unsubscribe from notifications.</returns>
    IDisposable Subscribe(Guid mainStatsId, Func<MainStats, Task> handler);
}
