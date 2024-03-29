﻿using Action.Handlers;

namespace Action.Events
{
    public class ApplicationShutdownEvent : Event
    {
        public override void Execute(IEventHandler eventHandler)
        {
            ((IEventHandlerApplicationShutdown)eventHandler).OnApplicationShutdown(this);
        }
    }
}