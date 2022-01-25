using Action.Handlers;
using Logging.Events;

namespace Logging.Handlers
{
    public interface IEventHandlerCatchAnException : IEventHandler
    {
        void OnCatchAnException(CatchAnExceptionEvent catchAnExceptionEvent);
    }
}