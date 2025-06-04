namespace Server.Module.Player.Api;

internal class PlayerSettings
{
    public required PostgreSql PostgreSql { get; set; }
}

public class PostgreSql
{
    public required string Host { get; set; }
    public required uint Port { get; set; }
    public required string Database { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

    /// <summary>
    /// Constructs and returns a PostgreSQL connection string using the configured properties.
    /// </summary>
    /// <returns>A PostgreSQL connection string based on the current settings.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <c>Host</c>, <c>Database</c>, or <c>Username</c> is null, empty, or consists only of whitespace.
    /// </exception>
    public string CreateConnectionString()
    {
        if (string.IsNullOrWhiteSpace(Host))
        {
            throw new ArgumentException("Host cannot be null or empty", nameof(Host));
        }
        if (string.IsNullOrWhiteSpace(Database))
        {
            throw new ArgumentException("Database cannot be null or empty", nameof(Database));
        }
        if (string.IsNullOrWhiteSpace(Username))
        {
            throw new ArgumentException("Username cannot be null or empty", nameof(Username));
        }

        Npgsql.NpgsqlConnectionStringBuilder builder = new()
        {
            Host = Host,
            Port = (int)Port,
            Database = Database,
            Username = Username,
            Password = Password,
        };
        return builder.ConnectionString;
    }
}
