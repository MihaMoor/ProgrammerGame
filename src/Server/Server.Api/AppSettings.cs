namespace Server.Api;

public class AppSettings
{
    public required PostgreSql PostgreSql { get; set; }
    public required Elasticsearch Elasticsearch { get; set; }
    public required Logstash Logstash { get; set; }
}

public class PostgreSql
{
    public required string Host { get; set; }
    public required uint Port { get; set; }
    public required string Database { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

    /// <summary>
    /// Constructs and returns a PostgreSQL connection string using the configured database parameters.
    /// </summary>
    /// <returns>A connection string suitable for initializing a PostgreSQL connection.</returns>
    public string CreateConnectionString()
    {
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

public class Logstash
{
    /// <summary>
    /// Адрес
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// Лимит объема сообщения в очереди
    /// </summary>
    public ulong QueueLimitBytes { get; set; }

    /// <summary>
    /// Получить url адрес подключения
    /// </summary>
    /// <summary>
/// Returns a <see cref="Uri"/> object constructed from the configured URL.
/// </summary>
/// <returns>A <see cref="Uri"/> representing the configured URL.</returns>
    public Uri GetUri() => new(Url);
}

public class Elasticsearch
{
    /// <summary>
    /// Адрес
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// Получить url адрес подключения
    /// </summary>
    /// <summary>
/// Returns a <see cref="Uri"/> object constructed from the configured URL.
/// </summary>
/// <returns>A <see cref="Uri"/> representing the configured URL.</returns>
    public Uri GetUri() => new(Url);
}
