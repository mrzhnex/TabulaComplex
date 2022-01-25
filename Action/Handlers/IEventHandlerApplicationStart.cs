using Action.Events;

namespace Action.Handlers
{
    public interface IEventHandlerApplicationStart : IEventHandler
    {
        void OnApplicationStart(ApplicationStartEvent applicationStartEvent);
    }
}