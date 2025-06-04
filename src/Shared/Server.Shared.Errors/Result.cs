namespace Server.Shared.Errors;

public class Result
{
    /// <summary>
    /// »нициализирует новый экземпл€р класса <see cref="Result"/>, обеспечива€ согласованность между состо€нием успеха и значением ошибки.
    /// </summary>
    /// <param name="isSuccess">”казывает, представл€ет ли результат успешную операцию.</param>
    /// <param name="error">ќшибка, св€занна€ с результатом, или <see cref="Error.None"/>, если операци€ успешна.</param>
    /// <exception cref="ArgumentException">
    /// ¬озникает, если <paramref name="isSuccess"/> равно true, а <paramref name="error"/> не равно <see cref="Error.None"/>,
    /// или если <paramref name="isSuccess"/> равно false, а <paramref name="error"/> равно <see cref="Error.None"/>.
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
    /// —оздает успешный экземпл€р <see cref="Result"/> без ошибки.
    /// </summary>
    /// <returns>Ёкземпл€р <see cref="Result"/>, представл€ющий успешный результат.</returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// —оздает неудачный экземпл€р <see cref="Result"/> с указанной ошибкой.
    /// </summary>
    /// <param name="error">ќшибка, св€занна€ с неудачей.</param>
    /// <returns>Ёкземпл€р <see cref="Result"/>, представл€ющий неудачу с заданной ошибкой.</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// —оздает успешный экземпл€р <see cref="Result{TValue}"/> с указанным значением, или неудачный результат, если значение равно null.
    /// </summary>
    /// <param name="value">«начение, которое будет включено в успешный результат.</param>
    /// <returns>”спешный результат с этим значением, или неудачный результат с <c>Error.NullValue</c>, если значение равно null.</returns>
    public static Result<TValue> Success<TValue>(TValue value) =>
    value is null ? Failure<TValue>(Error.NullValue) : new(value, true, Error.None);

    /// <summary>
    /// —оздает неудачный экземпл€р <see cref="Result{TValue}"/> с указанной ошибкой.
    /// </summary>
    /// <typeparam name="TValue">“ип значени€, которое должно было быть возвращено при успехе.</typeparam>
    /// <param name="error">ќшибка, описывающа€ причину неудачи.</param>
    /// <returns>Ќеудачный результат, содержащий указанную ошибку.</returns>
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// »нициализирует новый экземпл€р класса <see cref="Result{TValue}"/>, представл€ющий результат выполнени€ операции с св€занным значением.
    /// </summary>
    /// <param name="value">«начение, возвращенное операцией, если она успешна; иначе Ч null.</param>
    /// <param name="isSuccess">ѕоказывает, была ли операци€ успешной.</param>
    /// <param name="error">ќшибка, св€занна€ с неудачной операцией, или <see cref="Error.None"/>, если операци€ успешна.</param>
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
