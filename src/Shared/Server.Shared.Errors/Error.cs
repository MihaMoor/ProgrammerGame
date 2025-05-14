using Server.Shared.Results;

namespace Server.Shared.Errors;

public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

    public static implicit operator Result(Error error) => Result.Failure(error);

    /// <summary>
/// Converts this error into a failure <see cref="Result"/> containing the current error instance.
/// </summary>
/// <returns>A failure <see cref="Result"/> with this error.</returns>
public Result ToResult() => Result.Failure(this);
}
