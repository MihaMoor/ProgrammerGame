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
    /// Возвращает URL-адрес конечной точки Logstash в виде объекта <see cref="Uri"/>.
    /// </summary>
    /// <returns>Абсолютный <see cref="Uri"/>, разобранный из свойства <c>Url</c>.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если значение свойства <c>Url</c> не является допустимым абсолютным URL.</exception>
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
    /// Возвращает объект <see cref="Uri"/>, созданный на основе настроенного URL-адреса конечной точки Elasticsearch.
    /// </summary>
    /// <returns>Объект <see cref="Uri"/>, представляющий конечную точку Elasticsearch.</returns>
    public Uri GetUri() => new(Url);
}
