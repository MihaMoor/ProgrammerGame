using RealTimePrototype.Domain.EventServices;

namespace RealTimePrototype.API.HostedServices;

public class TimedHostedService(
    ILogger<TimedHostedService> logger,
    EventManagerService eventManagerService)
    : IHostedService, IDisposable
{
    private int executionCount = 0;
    private Timer? _timer = null;

    public Task StartAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(
            DoWork,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
    public void Dispose()
        => _timer?.Dispose();
}
