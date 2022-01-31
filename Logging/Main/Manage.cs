using System;
using System.IO;
using Logging.Basis;
using Logging.Events;
using Logging.Handlers;

namespace Logging.Main
{
    public class Manage
    {
        public static Manage ManageInstance { get; private set; } = new();
        public Info Info { get; private set; } = new();
        public Log Log { get; private set; } = new();
        public Application Application { get; private set; } = new();

        public string GetLogsFolder()
        {
            return Path.Combine(Info.DefaultFolderPath, Info.DefaultApplicationName, Info.DefaultFolderName);
        }
        internal string GetLogsFullPath(string logType)
        {
            return Path.Combine(GetLogsFolder(), logType);
        }
        public bool IsUsedByAnotherProcess(string filePath)
        {
            if (!File.Exists(filePath))
                return false;
            try
            {
                using Stream stream = new FileStream(filePath, FileMode.Open);
            }
            catch (Exception exception)
            {
                Action.Main.Manage.ManageInstance.ExecuteEvent<IEventHandlerCatchAnException>(new CatchAnExceptionEvent(nameof(IsUsedByAnotherProcess), exception, LogLevel.Debug));
                return true;
            }
            return false;
        }
    }
}