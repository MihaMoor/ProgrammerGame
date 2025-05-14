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
    private string _url = string.Empty;

    /// <summary>
    /// Адрес
    /// </summary>
    public required string Url
    {
        get => _url;
        set => _url = value;
    }

    /// <summary>
    /// Лимит объема сообщения в очереди
    /// </summary>
    public ulong QueueLimitBytes { get; set; }

    /// <summary>
    /// Получить url адрес подключения
    /// </summary>
    /// <returns>url</returns>
    public Uri GetUri() => new(_url);
}

public class Elasticsearch
{
    private string _url = string.Empty;

    /// <summary>
    /// Адрес
    /// </summary>
    public required string Url
    {
        set => _url = value;
    }

    /// <summary>
    /// Получить url адрес подключения
    /// </summary>
    /// <returns>url</returns>
    public Uri GetUri() => new(_url);
}
