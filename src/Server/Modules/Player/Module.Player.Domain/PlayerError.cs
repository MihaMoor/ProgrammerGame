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
}
