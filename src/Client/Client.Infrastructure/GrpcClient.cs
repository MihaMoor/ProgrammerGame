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
    /// �������������� ����� ��������� ������ <see cref="GrpcClient{TClient}"/> � ��������� gRPC-�������� � ������� �������.
    /// </summary>
    /// <param name="adress">����� gRPC-�������.</param>
    /// <param name="client">��������� gRPC-������� ��� �������������� � ��������.</param>
    /// <remarks>
    /// ����������� gRPC-����� � ������������� �������� ��������� ������� ��� ��������� ��������� ������,
    /// ����� ��� ������������� ������� ��� ���������� ������� ��������.
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
    /// ����������� ����������� �������, ������������ ����������� GrpcClient.
    /// </summary>
    /// <param name="disposing">
    /// �������� true ��������� ���������� ��� �����������, ��� � ������������� �������;
    /// �������� false - ������ ������������� �������.
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
        // �� ��������� ���� ���. ��������� ��� ������� � ����� 'Dispose(bool disposing)'
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // �� ��������� ���� ���. ��������� ��� ������� � ����� 'Dispose(bool disposing)'
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
