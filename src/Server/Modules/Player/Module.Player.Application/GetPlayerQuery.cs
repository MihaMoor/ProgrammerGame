namespace Module.Player.Application;

public record GetPlayerQuery(Guid Id) : IQuery;

public class GetPlayerQueryHandler(IPlayerRepository playerRepository) : IQueryHandler<GetPlayerQuery, Player>
{
    public async Task<Result<Player>> Handle(GetPlayerQuery query, CancellationToken cancellationToken)
    {
        Player? player = await playerRepository.GetAsync(query.Id, cancellationToken);
        if (player is null)
        {
            return Result.Failure<Player>(PlayerError.NotFound(query.Id));
        }
        return Result.Success(player);
    }
}

// что-то тут не то
//public record GetPlayerQuery2(): IQuery<string>;

public class GetPlayerQuery2Handler(IPlayerRepository playerRepository) : ITypedQueryHandler<Guid, Player>
{
    public async Task<Result<Player>> Handle(Guid id, CancellationToken cancellationToken)
    {
        Player? player = await playerRepository.GetAsync(id, cancellationToken);
        if (player is null)
        {
            return Result.Failure<Player>(PlayerError.NotFound(id));
        }
        return Result.Success(player);
    }
}

public class Player
{
}

public static class PlayerError
{
    public static Error NotFound(Guid playerId) => new(
        "Player.NotFound", $"The user with the Id = '{playerId}' was not found");
}

public interface IPlayerRepository
{
    /// <summary>
    /// Получение игрока по Id.
    /// </summary>
    /// <param name="id">Id игрока</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Игрока</returns>
    Task<Player?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ICommand
{
}

public interface ICommand<TResponse>
{
}

public interface IQuery
{
}

// не получится использовать
//public interface IQuery<TType>
//{
//}

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}

public interface ITypedQueryHandler<TQueryType, TResponse>
{
    Task<Result<TResponse>> Handle(TQueryType query, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}

public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

    public static implicit operator Result(Error error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);
}
