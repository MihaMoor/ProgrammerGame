using Grpc.Net.Client;

namespace Client.Infrastructure;

public class GrpcClient<TClient> : IDisposable
{
    protected readonly HttpClientHandler _handler;
    protected readonly GrpcChannel _channel;
    protected bool _disposedValue;

    protected TClient Client { get; }

    public GrpcClient(string adress, TClient client)
    {
        Client = client;
        _handler = new();
        _channel = GrpcChannel.ForAddress(
            adress,
            new GrpcChannelOptions { HttpHandler = _handler }
        );
    }

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
