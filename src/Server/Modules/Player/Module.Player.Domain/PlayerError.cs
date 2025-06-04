using Server.Shared.Errors;

namespace Server.Module.Player.Domain;

public static class PlayerError
{
    /// <summary>
        /// Creates an error indicating that a player with the specified ID was not found.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player.</param>
        /// <returns>An <see cref="Error"/> representing the not found condition for the player.</returns>
        public static Error NotFound(Guid playerId) =>
        new(
            ErrorCode.EntityNotFound,
            $"The player with the Id = '{playerId}' was not found"
        );

    /// <summary>
        /// Creates an error indicating that the player's name is empty.
        /// </summary>
        /// <returns>An <see cref="Error"/> with code <c>IsEmpty</c> and a message stating the player's name cannot be empty.</returns>
        public static Error NameIsEmpty() =>
        new(ErrorCode.IsEmpty, $"The player's name cannot be empty");

    /// <summary>
        /// Creates an error indicating that the player's health value is outside the valid range of 0 to 100.
        /// </summary>
        /// <returns>An <see cref="Error"/> with code <c>OutOfRange</c> and a descriptive message.</returns>
        public static Error HealthOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The health must be between 0 and 100");

    /// <summary>
        /// Creates an error indicating that the player's mood value is outside the valid range of 0 to 100.
        /// </summary>
        /// <returns>An <see cref="Error"/> with code <c>OutOfRange</c> and a descriptive message.</returns>
        public static Error MoodOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The mood must be between 0 and 100");

    /// <summary>
        /// Creates an error indicating that the player's hunger value is outside the valid range of 0 to 100.
        /// </summary>
        /// <returns>An <see cref="Error"/> with code <c>OutOfRange</c> and a descriptive message.</returns>
        public static Error HungerOutOfRange() =>
        new(ErrorCode.OutOfRange, $"The hunger must be between 0 and 100");

    /// <summary>
        /// Creates an error indicating that player initialization failed, aggregating the codes and descriptions of the provided errors.
        /// </summary>
        /// <param name="errors">A list of errors encountered during initialization.</param>
        /// <returns>An <see cref="Error"/> with code <c>InitializationFailed</c> and a combined description of all provided errors.</returns>
        public static Error InitializationFailed(List<Error> errors) =>
        new(ErrorCode.InitializationFailed, string.Join("\n", errors.Select(x => $"{x.Code}: {x.Description}")));
}
