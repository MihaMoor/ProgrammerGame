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
    /// <returns>url</returns>
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
    /// <returns>url</returns>
    public Uri GetUri() => new(Url);
}
