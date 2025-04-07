using Grpc.Core;
using Grpc.Net.Client;

namespace Client.Infrastructure;

public class GrpcClient<TRequest, TResponce> : ClientBase, IDisposable
{
    private HttpClientHandler _handler;
    private GrpcChannel _channel;

    public TResponce Client { get; }

    private bool disposedValue;

    public GrpcClient(string adress, TResponce client)
    {
        Client = client;
        _handler = new();
        _channel = GrpcChannel.ForAddress(adress, new GrpcChannelOptions
        {
            HttpHandler = _handler
        });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _channel.Dispose();
                _handler.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
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
