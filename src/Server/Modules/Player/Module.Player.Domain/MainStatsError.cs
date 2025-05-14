namespace Server.Module.Player.Application;

public static class MainStatsError
{
    /// <summary>
        /// Creates an error indicating that main stats with the specified ID were not found.
        /// </summary>
        /// <param name="mainStatsId">The unique identifier of the main stats.</param>
        /// <returns>An <see cref="Error"/> representing the not found condition for the given main stats ID.</returns>
        public static Error NotFound(Guid mainStatsId) =>
        new(
            $"{nameof(MainStatsError)}.{nameof(NotFound)}",
            $"The main stats with the Id = '{mainStatsId}' was not found"
        );

    /// <summary>
        /// Creates an error indicating that the player's name is missing.
        /// </summary>
        /// <returns>An <see cref="Error"/> representing the empty player name condition.</returns>
        public static Error NameIsEmpty() =>
        new(
            $"{nameof(MainStatsError)}.{nameof(NameIsEmpty)}",
            $"The player's name cannot be empty"
        );
}
