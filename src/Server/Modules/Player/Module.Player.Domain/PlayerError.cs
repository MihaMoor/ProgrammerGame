using Server.Shared.Errors;

namespace Server.Module.Player.Domain;

public static class PlayerError
{
    /// <summary>
    /// Создает ошибку, указывающую, что игрок с указанным идентификатором не найден.
    /// </summary>
    /// <param name="playerId">Уникальный идентификатор игрока.</param>
    /// <returns>Ошибка с кодом <c>EntityNotFound</c> и описательным сообщением.</returns>
    public static Error NotFound(Guid playerId) =>
        new(
            ErrorCode.EntityNotFound,
            $"The player with the Id = '{playerId}' was not found"
        );

    /// <summary>
    /// Создает ошибку, указывающую, что имя игрока пусто.
    /// </summary>
    /// <returns>Ошибка с кодом <c>IsEmpty</c> и описательным сообщением.</returns>
    public static Error NameIsEmpty() =>
        new(ErrorCode.IsEmpty, $"The player's name cannot be empty");

    /// <summary>
    /// Создает ошибку, указывающую, что значение здоровья игрока находится вне допустимого диапазона от 0 до 100.
    /// </summary>
    /// <returns>Ошибка <see cref="Error"/> с кодом ошибки <c>OutOfRange</c> и описательным сообщением.</returns>
    public static Error HealthOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The health must be between 0 and 100");

    /// <summary>
    /// Создает ошибку, указывающую, что значение настроения игрока находится вне допустимого диапазона (от 0 до 100).
    /// </summary>
    /// <returns>Ошибка с кодом <c>OutOfRange</c> и описательным сообщением.</returns>
    public static Error MoodOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The mood must be between 0 and 100");

    /// <summary>
    /// Создает ошибку, указывающую, что значение голода игрока находится вне допустимого диапазона (от 0 до 100).
    /// </summary>
    /// <returns>
    /// Ошибка <see cref="Error"/> с кодом ошибки <c>OutOfRange</c> и описательным сообщением.
    /// </returns>
    public static Error HungerOutOfRange() =>
    new(ErrorCode.OutOfRange, $"The hunger must be between 0 and 100");

    /// <summary>
    /// Создает ошибку, указывающую на сбой инициализации игрока, объединяя описания из нескольких ошибок.
    /// </summary>
    /// <param name="errors">Список ошибок, возникших в процессе инициализации игрока.</param>
    /// <returns>
    /// Ошибка с кодом <c>InitializationFailed</c> и объединенным описанием всех предоставленных ошибок.
    /// </returns>
    public static Error InitializationFailed(List<Error> errors)
        => new(
            ErrorCode.InitializationFailed,
            string.Join("\n", errors.Select(x => $"{x.Code}: {x.Description}"))
            );
}
