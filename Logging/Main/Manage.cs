using System.Globalization;
using System;
using System.IO;
using Logging.Core;

namespace Logging.Main
{
    public static class Manage
    {
        public static Log Log { get; set; } = new Log();
        public static string GetLogsFolder()
        {
            return Path.Combine(Info.DefaultFolderPath, Info.DefaultApplicationName, Info.DefaultFolderName);
        }
        internal static string GetLogsFullPath(string logType)
        {
            return Path.Combine(GetLogsFolder(), logType);
        }
        internal static string ConstructStringLog(string message, LogLevel logLevel)
        {
            return $"[{logLevel}][{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}]:{message}";
        }
        internal static bool IsUsedByAnotherProcess(string filePath)
        {
            if (!File.Exists(filePath))
                return false;
            try { using Stream stream = new FileStream(filePath, FileMode.Open); }
            catch { return true; }
            return false;
        }
    }
}