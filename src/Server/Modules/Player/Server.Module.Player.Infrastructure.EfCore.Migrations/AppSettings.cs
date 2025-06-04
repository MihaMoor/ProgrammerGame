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
    /// Создает и возвращает строку подключения к PostgreSQL, используя настроенные параметры хоста, порта, базы данных, имени пользователя и пароля.
    /// </summary>
    /// <returns>Строка подключения к PostgreSQL на основе текущих настроек.</returns>
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
    /// Возвращает объект <see cref="Uri"/>, созданный из настроенного URL.
    /// </summary>
    /// <returns>Объект <see cref="Uri"/>, представляющий URL конечной точки.</returns>
    public Uri GetUri() => new(Url);
}

public class Elasticsearch
{
    /// <summary>
    /// Адрес
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// Возвращает объект <see cref="Uri"/>, созданный на основе настроенного URL.
    /// </summary>
    /// <returns>Объект <see cref="Uri"/>, представляющий URL конечной точки.</returns>
    public Uri GetUri() => new(Url);
}
