namespace Server.Domain;

public class User
{
    private readonly IList<Player> _players = [];

    public Guid Id { get; set; }
    public IEnumerable<Player> Players => _players;

    public bool TryAddPlayer(Player player)
    {
        _players.Add(player);
        return true;
    }

    public bool TryRemovePlayer(Guid playerId)
    {
        Player? player = _players.Where(x => x.Id == playerId).FirstOrDefault();

        if (player == null)
            return false;

        return _players.Remove(player);
    }
}
