namespace Server.Api;

public class AppSettings
{
    public required Elasticsearch Elasticsearch { get; set; }
    public required Logstash Logstash { get; set; }
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
        set
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out _))
                throw new ArgumentException($"Неверный формат URL: {value}");
            _url = value;
        }
    }

    /// <summary>
    /// Лимит объема сообщения в очереди
    /// </summary>
    public ulong QueueLimitBytes { get; set; }

    /// <summary>
    /// Получить url адрес подключения
    /// </summary>
    /// <summary>
    /// Returns the Logstash endpoint URL as a <see cref="Uri"/>.
    /// </summary>
    /// <returns>The absolute <see cref="Uri"/> parsed from the <c>Url</c> property.</returns>
    /// <exception cref="ArgumentException">Thrown if the <c>Url</c> property is not a valid absolute URL.</exception>
    public Uri GetUri()
    {
        if (!Uri.TryCreate(Url, UriKind.Absolute, out var uri))
            throw new ArgumentException($"Неверный формат URL: {Url}");
        return uri;
    }
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
/// Returns a <see cref="Uri"/> object constructed from the configured Elasticsearch endpoint URL.
/// </summary>
/// <returns>A <see cref="Uri"/> representing the Elasticsearch endpoint.</returns>
    public Uri GetUri() => new(Url);
}
