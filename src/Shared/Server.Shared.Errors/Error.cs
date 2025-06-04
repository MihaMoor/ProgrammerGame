namespace Server.Shared.Errors;

public record Error(ErrorCode Code, string Description)
{
    public static readonly Error None = new(ErrorCode.None, string.Empty);
    public static readonly Error NullValue = new(ErrorCode.NullValue, "Null value was provided");

    public static implicit operator Result(Error error) => Result.Failure(error);

    /// <summary>
/// Converts this error into a failure <see cref="Result"/> containing the current error.
/// </summary>
/// <returns>A failure <see cref="Result"/> with this error.</returns>
public Result ToResult() => Result.Failure(this);
}
