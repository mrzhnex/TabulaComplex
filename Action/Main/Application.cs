using System.Collections.Generic;
using System;
using Action.Handlers;

namespace Action.Main
{
    public class Application : IEventHandler
	{
		public Application()
        {
			RegisterAll(this);
        }

		protected void RegisterAll(IEventHandler eventHandler)
		{
			foreach (Type type in eventHandler.GetType().GetInterfaces())
			{
				if (typeof(IEventHandler).IsAssignableFrom(type) && !Manage.ManageInstance.Events.ContainsKey(type))
					Manage.ManageInstance.Events.Add(type, new List<IEventHandler>() { eventHandler });
			}
		}
	}
}