using Grpc.Core;
using Grpc.Net.Client;
using Polly;

namespace Client.Infrastructure;

public class GrpcClient<TClient> : IDisposable
{
    protected readonly HttpClientHandler _handler;
    protected readonly GrpcChannel _channel;
    protected bool _disposedValue;

    protected IAsyncPolicy RetryPolicy { get; private set; }

    protected TClient Client { get; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GrpcClient{TClient}"/> с указанным gRPC-клиентом и адресом сервера.
    /// </summary>
    /// <param name="adress">Адрес gRPC-сервера.</param>
    /// <param name="client">Экземпляр gRPC-клиента для взаимодействия с сервером.</param>
    /// <remarks>
    /// Настраивает gRPC-канал и конфигурирует политику повторных попыток для обработки временных ошибок,
    /// таких как недоступность сервера или превышение времени ожидания.
    /// </remarks>
    public GrpcClient(string adress, TClient client)
    {
        Client = client;
        _handler = new();
        _channel = GrpcChannel.ForAddress(
            adress,
            new GrpcChannelOptions
            {
                HttpHandler = _handler,
            }
        );

        RetryPolicy = Policy
            .Handle<RpcException>(ex => ex.StatusCode == StatusCode.Unavailable ||
                ex.StatusCode == StatusCode.DeadlineExceeded)
            .WaitAndRetryAsync(
                retryCount: 10,
                sleepDurationProvider: retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)),
                onRetry: (exception, delay, retryCount, context) =>
                {
                    // Log retry attempts if needed
                    Console.WriteLine($"Retry {retryCount} after {delay.TotalSeconds} seconds due to: {exception.Message}");
                });
    }

    /// <summary>
    /// Освобождает управляемые ресурсы, используемые экземпляром GrpcClient.
    /// </summary>
    /// <param name="disposing">
    /// Значение true позволяет освободить как управляемые, так и неуправляемые ресурсы;
    /// значение false - только неуправляемые ресурсы.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _channel.Dispose();
                _handler.Dispose();
            }

            _disposedValue = true;
        }
    }

    ~GrpcClient()
    {
        // Не изменяйте этот код. Поместите код очистки в метод 'Dispose(bool disposing)'
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Не изменяйте этот код. Поместите код очистки в метод 'Dispose(bool disposing)'
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
