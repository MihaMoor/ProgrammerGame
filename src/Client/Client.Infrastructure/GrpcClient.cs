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
    /// Initializes a new instance of the <see cref="GrpcClient{TClient}"/> class with the specified gRPC client and server address.
    /// </summary>
    /// <param name="adress">The address of the gRPC server.</param>
    /// <param name="client">The gRPC client instance to encapsulate.</param>
    /// <remarks>
    /// Sets up the gRPC channel and configures an asynchronous retry policy for handling transient gRPC errors with exponential backoff.
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
    /// Releases the managed resources used by the <see cref="GrpcClient{TClient}"/> instance.
    /// </summary>
    /// <param name="disposing">
    /// True to release both managed and unmanaged resources; false to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _channel.Dispose();
                _handler.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~GrpcClient()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
