using System.Collections.Generic;
using System;
using Action.Events;
using Action.Handlers;

namespace Action.Main
{
    public class Manage
    {
        public static Manage ManageInstance { get; private set; } = new();
        public Info Info { get; private set; } = new();
		internal Dictionary<Type, List<IEventHandler>> Events { get; private set; } = new();

		public void ExecuteEvent<T>(Event @event) where T : IEventHandler
		{
			if (!Info.ShouldExecuteEvent || !Events.ContainsKey(typeof(T)))
				return;
			foreach (IEventHandler eventHandler in Events[typeof(T)])
				@event.Execute(eventHandler);
		}
		public void RegisterAllEvents(IEventHandler eventHandler)
        {
			foreach (Type type in eventHandler.GetType().GetInterfaces())
			{
				if (typeof(IEventHandler).IsAssignableFrom(type) && !Events.ContainsKey(type))
					Events.Add(type, new List<IEventHandler>() { eventHandler });
			}
		}
	}
}