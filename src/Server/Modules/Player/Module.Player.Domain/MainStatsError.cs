namespace Server.Module.Player.Application;

public static class MainStatsError
{
    public static Error NotFound(Guid mainStatsId) =>
        new(
            $"{nameof(MainStatsError)}.{nameof(NotFound)}",
            $"The main stats with the Id = '{mainStatsId}' was not found"
        );

    public static Error NameIsEmpty() =>
        new(
            $"{nameof(MainStatsError)}.{nameof(NameIsEmpty)}",
            $"The player's name cannot be empty"
        );
}
