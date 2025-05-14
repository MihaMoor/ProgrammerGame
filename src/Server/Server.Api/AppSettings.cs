namespace Server.Api;

public class AppSettings
{
    public required string PostgreSqlConnectionString { get; set; }
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
        /// Builds and returns a PostgreSQL connection string using the configured host, port, database, username, and password.
        /// </summary>
        /// <returns>A connection string formatted for PostgreSQL.</returns>
        public string CreateConnectionString() =>
        $"Host={Host}; Port={Port}; Database={Database}; Username={Username}; Password={Password}";
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
/// Returns a <see cref="Uri"/> instance constructed from the stored URL.
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
/// Returns a <see cref="Uri"/> instance constructed from the configured URL.
/// </summary>
/// <returns>A <see cref="Uri"/> representing the service endpoint.</returns>
    public Uri GetUri() => new(Url);
}
