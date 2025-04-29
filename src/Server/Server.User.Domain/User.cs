namespace Server.User.Domain;

public class User
{
    private readonly IList<Player> _players = [];

    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// Список персонажей пользователя
    /// </summary>
    public IEnumerable<Guid> Players => _players.Select(x => x.Id);

    /// <summary>
    /// Добавление нового персонажа
    /// </summary>
    /// <param name="player">Персонаж</param>
    /// <returns>true если получилось, false если нет</returns>
    public bool TryAddPlayer(Player player)
    {
        if (_players.Where(x => x.Id == player.Id).Any())
            return false;

        _players.Add(player);
        return true;
    }

    /// <summary>
    /// Удалениее персонажа
    /// </summary>
    /// <param name="playerId">Id персонажа</param>
    /// <returns></returns>
    public bool TryRemovePlayer(Guid playerId)
    {
        Player? p = _players.Where(x => x.Id == playerId).FirstOrDefault();

        if (p == null)
            return false;

        return _players.Remove(p);
    }
}
