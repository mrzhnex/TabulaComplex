using Action.Handlers;

namespace Action.Events
{
    public abstract class Event
    {
        public abstract void Execute(IEventHandler eventHandler);
    }
}