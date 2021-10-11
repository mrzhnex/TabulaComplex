using Action.Events;

namespace Action.Handlers
{
    public interface IEventHandlerApplicationShutdown : IEventHandler
    {
        void OnApplicationShutdown(ApplicationShutdownEvent applicationShutdownEvent);
    }
}