namespace RealTimePrototype.Domain.EventServices;

public class EventManagerService(HungryEventService hungryEventService)
{
    public void React()
    {
        hungryEventService.React();
    }
}
