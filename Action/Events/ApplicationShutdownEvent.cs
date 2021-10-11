using Action.Handlers;

namespace Action.Events
{
    public class ApplicationShutdownEvent : Event
    {
        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerApplicationShutdown)handler).OnApplicationShutdown(this);
        }
    }
}