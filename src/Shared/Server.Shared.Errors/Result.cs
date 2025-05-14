using Server.Shared.Errors;

namespace Server.Shared.Results;

public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class, enforcing consistency between success state and error value.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result represents a successful operation.</param>
    /// <param name="error">The error associated with a failure, or <see cref="Error.None"/> for success.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="isSuccess"/> is true and <paramref name="error"/> is not <see cref="Error.None"/>,
    /// or if <paramref name="isSuccess"/> is false and <paramref name="error"/> is <see cref="Error.None"/>.
    /// </exception>
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    /// <summary>
/// Creates a successful <see cref="Result"/> with no error.
/// </summary>
/// <returns>A <see cref="Result"/> representing a successful outcome.</returns>
public static Result Success() => new(true, Error.None);

    /// <summary>
/// Creates a failed <see cref="Result"/> with the specified error.
/// </summary>
/// <param name="error">The error describing the reason for failure.</param>
/// <returns>A <see cref="Result"/> representing a failure.</returns>
public static Result Failure(Error error) => new(false, error);

    /// <summary>
/// Creates a successful <see cref="Result{TValue}"/> containing the specified value.
/// </summary>
/// <param name="value">The value to associate with the successful result.</param>
/// <returns>A successful result containing the provided value.</returns>
public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    /// <summary>
/// Creates a failed <see cref="Result{TValue}"/> with the specified error.
/// </summary>
/// <param name="error">The error describing the reason for failure.</param>
/// <returns>A failure result containing the provided error.</returns>
public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class with the specified value, success state, and error.
    /// </summary>
    /// <param name="value">The value associated with a successful result, or null for a failure.</param>
    /// <param name="isSuccess">Indicates whether the result represents a success.</param>
    /// <param name="error">The error associated with a failure, or <c>Error.None</c> for success.</param>
    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "The value of a failure result can't be accessed"
            );

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}
