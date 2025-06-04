using Server.Shared.Errors;

namespace Server.Module.Player.Domain;

public static class PlayerError
{
    /// <summary>
    /// ������� ������, �����������, ��� ����� � ��������� ��������������� �� ������.
    /// </summary>
    /// <param name="playerId">���������� ������������� ������.</param>
    /// <returns>������ � ����� <c>EntityNotFound</c> � ������������ ����������.</returns>
    public static Error NotFound(Guid playerId) =>
        new(
            ErrorCode.EntityNotFound,
            $"The player with the Id = '{playerId}' was not found"
        );

    /// <summary>
    /// ������� ������, �����������, ��� ��� ������ �����.
    /// </summary>
    /// <returns>������ � ����� <c>IsEmpty</c> � ������������ ����������.</returns>
    public static Error NameIsEmpty() =>
        new(ErrorCode.IsEmpty, $"The player's name cannot be empty");

    /// <summary>
    /// ������� ������, �����������, ��� �������� �������� ������ ��������� ��� ����������� ��������� �� 0 �� 100.
    /// </summary>
    /// <returns>������ <see cref="Error"/> � ����� ������ <c>OutOfRange</c> � ������������ ����������.</returns>
    public static Error HealthOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The health must be between 0 and 100");

    /// <summary>
    /// ������� ������, �����������, ��� �������� ���������� ������ ��������� ��� ����������� ��������� (�� 0 �� 100).
    /// </summary>
    /// <returns>������ � ����� <c>OutOfRange</c> � ������������ ����������.</returns>
    public static Error MoodOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The mood must be between 0 and 100");

    /// <summary>
    /// ������� ������, �����������, ��� �������� ������ ������ ��������� ��� ����������� ��������� (�� 0 �� 100).
    /// </summary>
    /// <returns>
    /// ������ <see cref="Error"/> � ����� ������ <c>OutOfRange</c> � ������������ ����������.
    /// </returns>
    public static Error HungerOutOfRange() =>
    new(ErrorCode.OutOfRange, $"The hunger must be between 0 and 100");

    /// <summary>
    /// ������� ������, ����������� �� ���� ������������� ������, ��������� �������� �� ���������� ������.
    /// </summary>
    /// <param name="errors">������ ������, ��������� � �������� ������������� ������.</param>
    /// <returns>
    /// ������ � ����� <c>InitializationFailed</c> � ������������ ��������� ���� ��������������� ������.
    /// </returns>
    public static Error InitializationFailed(List<Error> errors)
        => new(
            ErrorCode.InitializationFailed,
            string.Join("\n", errors.Select(x => $"{x.Code}: {x.Description}"))
            );
}
