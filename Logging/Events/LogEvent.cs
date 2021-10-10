using Action.Events;
using Action.Handlers;
using Logging.Core;
using Logging.Handlers;

namespace Logging.Events
{
    public class LogEvent : Event
    {
        public string Message { get; set; } = string.Empty;
        public string LogType { get; set; } = string.Empty;
        public LogLevel LogLevel { get; set; } = LogLevel.Trace;
        public LogEvent(string Message, string LogType, LogLevel LogLevel)
        {
            this.Message = Message;
            this.LogType = LogType;
            this.LogLevel = LogLevel;
        }
        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerLog)handler).OnLog(this);
        }
    }
}