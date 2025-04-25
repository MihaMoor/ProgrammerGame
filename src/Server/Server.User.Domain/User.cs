namespace Server.User.Domain;

public class User
{
    private readonly IList<Guid> _players = [];

    public Guid Id { get; set; }
    public IEnumerable<Guid> Players => _players;

    public bool TryAddPlayer(Guid player)
    {
        if (_players.Contains(player))
            return false;

        _players.Add(player);
        return true;
    }

    public bool TryRemovePlayer(Guid playerId)
    {
        Guid? player = _players.Where(x => x == playerId).FirstOrDefault();

        if (player == null)
            return false;

        return _players.Remove((Guid)player);
    }
}
