using Action.Handlers;
using Logging.Events;

namespace Logging.Handlers
{
    internal interface IEventHandlerLog : IEventHandler
    {
        void OnLog(LogEvent logEvent);
    }
}