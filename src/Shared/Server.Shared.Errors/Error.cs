namespace Server.Shared.Errors;

public record Error(ErrorCode Code, string Description)
{
    public static readonly Error None = new(ErrorCode.None, string.Empty);
    public static readonly Error NullValue = new(ErrorCode.NullValue, "Null value was provided");

    public static implicit operator Result(Error error) => Result.Failure(error);

    /// <summary>
    /// Преобразует эту ошибку в неудачный <see cref="Result"/>, содержащий текущую ошибку.
    /// </summary>
    /// <returns>Неудачный <see cref="Result"/> с этой ошибкой.</returns>
    public Result ToResult() => Result.Failure(this);
}
