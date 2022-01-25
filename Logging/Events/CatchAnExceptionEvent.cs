using System;
using Action.Events;
using Action.Handlers;
using Logging.Basis;
using Logging.Handlers;

namespace Logging.Events
{
    public class CatchAnExceptionEvent : Event
    {
        public string Message { get; private set; } = string.Empty;
        public Exception Exception { get; private set; } = new Exception();
        public LogLevel LogLevel { get; private set; } = LogLevel.Error;

        public CatchAnExceptionEvent(string Message, Exception Exception, LogLevel LogLevel)
        {
            this.Message = Message;
            this.Exception = Exception;
            this.LogLevel = LogLevel;
        }

        public override void Execute(IEventHandler eventHandler)
        {
            ((IEventHandlerCatchAnException)eventHandler).OnCatchAnException(this);
        }
    }
}