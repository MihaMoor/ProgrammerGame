using Server.Shared.Errors;

namespace Server.Module.Player.Domain;

public static class PlayerError
{
    /// <summary>
        /// Creates an error indicating that a player with the specified ID was not found.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <returns>An <see cref="Error"/> representing a player not found error.</returns>
        public static Error NotFound(Guid playerId) =>
        new(
            ErrorCode.EntityNotFound,
            $"The player with the Id = '{playerId}' was not found"
        );

    /// <summary>
        /// Creates an error indicating that the player's name is missing or empty.
        /// </summary>
        /// <returns>An <see cref="Error"/> with code <c>IsEmpty</c> and a descriptive message.</returns>
        public static Error NameIsEmpty() =>
        new(ErrorCode.IsEmpty, $"The player's name cannot be empty");
}
