namespace Server.Shared.Errors;

public class Result
{
    /// <summary>
    /// �������������� ����� ��������� ������ <see cref="Result"/>, ����������� ��������������� ����� ���������� ������ � ��������� ������.
    /// </summary>
    /// <param name="isSuccess">���������, ������������ �� ��������� �������� ��������.</param>
    /// <param name="error">������, ��������� � �����������, ��� <see cref="Error.None"/>, ���� �������� �������.</param>
    /// <exception cref="ArgumentException">
    /// ���������, ���� <paramref name="isSuccess"/> ����� true, � <paramref name="error"/> �� ����� <see cref="Error.None"/>,
    /// ��� ���� <paramref name="isSuccess"/> ����� false, � <paramref name="error"/> ����� <see cref="Error.None"/>.
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
    /// ������� �������� ��������� <see cref="Result"/> ��� ������.
    /// </summary>
    /// <returns>��������� <see cref="Result"/>, �������������� �������� ���������.</returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// ������� ��������� ��������� <see cref="Result"/> � ��������� �������.
    /// </summary>
    /// <param name="error">������, ��������� � ��������.</param>
    /// <returns>��������� <see cref="Result"/>, �������������� ������� � �������� �������.</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// ������� �������� ��������� <see cref="Result{TValue}"/> � ��������� ���������, ��� ��������� ���������, ���� �������� ����� null.
    /// </summary>
    /// <param name="value">��������, ������� ����� �������� � �������� ���������.</param>
    /// <returns>�������� ��������� � ���� ���������, ��� ��������� ��������� � <c>Error.NullValue</c>, ���� �������� ����� null.</returns>
    public static Result<TValue> Success<TValue>(TValue value) =>
    value is null ? Failure<TValue>(Error.NullValue) : new(value, true, Error.None);

    /// <summary>
    /// ������� ��������� ��������� <see cref="Result{TValue}"/> � ��������� �������.
    /// </summary>
    /// <typeparam name="TValue">��� ��������, ������� ������ ���� ���� ���������� ��� ������.</typeparam>
    /// <param name="error">������, ����������� ������� �������.</param>
    /// <returns>��������� ���������, ���������� ��������� ������.</returns>
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// �������������� ����� ��������� ������ <see cref="Result{TValue}"/>, �������������� ��������� ���������� �������� � ��������� ���������.
    /// </summary>
    /// <param name="value">��������, ������������ ���������, ���� ��� �������; ����� � null.</param>
    /// <param name="isSuccess">����������, ���� �� �������� ��������.</param>
    /// <param name="error">������, ��������� � ��������� ���������, ��� <see cref="Error.None"/>, ���� �������� �������.</param>
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
