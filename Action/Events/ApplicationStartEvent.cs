using Action.Handlers;

namespace Action.Events
{
    public class ApplicationStartEvent : Event
    {
        public override void Execute(IEventHandler eventHandler)
        {
            ((IEventHandlerApplicationStart)eventHandler).OnApplicationStart(this);
        }
    }
}