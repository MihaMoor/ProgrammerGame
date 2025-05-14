using Server.Shared.Errors;

namespace Server.Module.Player.Domain;

public static class MainStatsError
{
    /// <summary>
        /// Creates an error indicating that the main stats entity with the specified ID was not found.
        /// </summary>
        /// <param name="mainStatsId">The unique identifier of the main stats entity.</param>
        /// <returns>An <see cref="Error"/> representing the not found error for the given main stats ID.</returns>
        public static Error NotFound(Guid mainStatsId) =>
        new(
            $"{nameof(MainStatsError)}.{nameof(NotFound)}",
            $"The main stats with the Id = '{mainStatsId}' was not found"
        );

    /// <summary>
        /// Creates an error indicating that the player's name is missing or empty.
        /// </summary>
        /// <returns>An <see cref="Error"/> representing the validation failure for an empty player name.</returns>
        public static Error NameIsEmpty() =>
        new(
            $"{nameof(MainStatsError)}.{nameof(NameIsEmpty)}",
            $"The player's name cannot be empty"
        );
}
