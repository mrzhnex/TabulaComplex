using Action.Handlers;
using Logging.Events;

namespace Logging.Handlers
{
    public interface IEventHandlerLog : IEventHandler
    {
        void OnLog(LogEvent logEvent);
    }
}