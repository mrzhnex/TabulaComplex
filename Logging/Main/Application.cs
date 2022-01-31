using Action.Events;
using Action.Handlers;

namespace Logging.Main
{
    public class Application : Action.Main.Application, IEventHandlerApplicationShutdown
    {
        public void OnApplicationShutdown(ApplicationShutdownEvent applicationShutdownEvent)
        {
            Manage.ManageInstance.Info.ActiveLog = false;
        }
    }
}