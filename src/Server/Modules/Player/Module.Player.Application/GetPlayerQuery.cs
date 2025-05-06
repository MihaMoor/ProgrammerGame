namespace Module.Player.Application;

public interface IPlayerWriteRepository { }
public interface IPlayerReadRepository { }

public interface ICommandHandler { }
public interface IQuery { }
public interface IQueryHandler<Tout, Tin> where Tin : IQuery
{
    Task<Tout> Handle(Tin query, CancellationToken roken = default);
}

public class GetPlayerQuery : IQuery
{
    public Guid Id { get; set; }
}

public class GetPlayerQueryHandler(IPlayerWriteRepository writeRepository) : IQueryHandler<Player, GetPlayerQuery>
{
    public Task<Player> Handle(GetPlayerQuery query, CancellationToken roken = default)
    {
        throw new NotImplementedException();
    }
}

public class Player
{

}
