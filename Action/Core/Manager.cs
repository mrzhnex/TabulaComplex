using System;
using System.Collections.Generic;
using Action.Events;
using Action.Handlers;
using Action.Main;

namespace Action.Core
{
    public class Manager
	{
		private Dictionary<Type, List<IEventHandler>> EventMeta { get; set; } = new Dictionary<Type, List<IEventHandler>>();

		internal Manager() { }

		public void ExecuteEvent<T>(Event ev) where T : IEventHandler
		{
			if (!EventMeta.ContainsKey(typeof(T)) || !Info.ExecuteEvent)
				return;
			foreach (IEventHandler eventHandler in EventMeta[typeof(T)])
			{
				ev.ExecuteHandler(eventHandler);
			}
		}
		public void AddEventHandlers(IEventHandler handler)
		{
			foreach (Type type in handler.GetType().GetInterfaces())
			{
				if (typeof(IEventHandler).IsAssignableFrom(type))
				{
					AddEventHandler(type, handler);
				}
			}
		}
		private void AddEventHandler(Type eventType, IEventHandler handler)
		{
			if (!EventMeta.ContainsKey(eventType))
			{
				EventMeta.Add(eventType, new List<IEventHandler>
				{
					handler
				});
				return;
			}
		}
	}
}