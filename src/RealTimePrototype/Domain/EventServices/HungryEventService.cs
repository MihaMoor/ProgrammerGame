using RealTimePrototype.Domain.Abstractions;

namespace RealTimePrototype.Domain.EventServices;

public class HungryEventService : IEventService
{
    public event Action HungryEvent;

    public void React()
        => HungryEvent?.Invoke();
}
