using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace Client.Infrastructure;

public class GrpcClient : IDisposable
{
    private GrpcWebHandler _webHandler;
    private GrpcChannel _channel;

    private bool disposedValue;

    public GrpcClient(string adress)
    {
        _webHandler = new (new HttpClientHandler());
        _channel = GrpcChannel.ForAddress(adress, new GrpcChannelOptions
        {
            HttpHandler = _webHandler
        });
    }

    public void SendMessage<TMessage, TClient>(TMessage message)
    {

    }

    public void RecieveMessage<T>()
    {

    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _channel.Dispose();
                _webHandler.Dispose();
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
