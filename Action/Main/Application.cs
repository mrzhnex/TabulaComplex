using System;
using Action.Handlers;

namespace Action.Main
{
    public class Application : IEventHandler
	{
		public Application()
        {
			foreach (Type type in GetType().GetInterfaces())
			{
				if (typeof(IEventHandler).IsAssignableFrom(type))
				{
					if (Manage.ManageInstance.Events.ContainsKey(type))
						Manage.ManageInstance.Events[type].Add(this);
					else
						Manage.ManageInstance.Events.Add(type, new() { this });
				}
			}
        }
	}
}