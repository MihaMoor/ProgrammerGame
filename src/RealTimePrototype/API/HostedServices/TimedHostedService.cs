using RealTimePrototype.Domain.Services;

namespace RealTimePrototype.API.HostedServices;

public class TimedHostedService(HungryService hungryService)
    : IHostedService, IDisposable
{
    private Timer? _timer = null;

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(
            DoWork,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
        => hungryService.Hungry();

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
    public void Dispose()
        => _timer?.Dispose();
}
