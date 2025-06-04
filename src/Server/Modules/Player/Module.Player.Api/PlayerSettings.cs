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
