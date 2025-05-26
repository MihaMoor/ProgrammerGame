using Server.Shared.Errors;

namespace Server.Module.Player.Domain;

public static class PlayerError
{
    public static Error NotFound(Guid playerId) =>
        new(
            ErrorCode.EntityNotFound,
            $"The player with the Id = '{playerId}' was not found"
        );

    public static Error NameIsEmpty() =>
        new(ErrorCode.IsEmpty, $"The player's name cannot be empty");

    public static Error HealthOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The health must be between 0 and 100");

    public static Error MoodOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The mood must be between 0 and 100");

    public static Error HungerOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The hunger must be between 0 and 100");

    public static Error InitializationFailed(List<Error> errors) =>
        new(ErrorCode.InitializationFailed, string.Join("\n", errors.Select(x => $"{x.Code}: {x.Description}")));
}
