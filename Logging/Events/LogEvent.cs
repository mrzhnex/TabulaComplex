using Action.Events;
using Action.Handlers;
using Logging.Basis;
using Logging.Handlers;

namespace Logging.Events
{
    public class LogEvent : Event
    {
        public string Message { get; private set; } = string.Empty;
        public string LogType { get; private set; } = string.Empty;
        public LogLevel LogLevel { get; private set; } = LogLevel.Trace;

        public LogEvent(string Message, string LogType, LogLevel LogLevel)
        {
            this.Message = Message;
            this.LogType = LogType;
            this.LogLevel = LogLevel;
        }

        public override void Execute(IEventHandler eventHandler)
        {
            ((IEventHandlerLog)eventHandler).OnLog(this);
        }
    }
}