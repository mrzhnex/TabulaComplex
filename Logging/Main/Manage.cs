using System.Globalization;
using System;
using System.IO;
using Logging.Core;

namespace Logging.Main
{
    public static class Manage
    {
        public static string GetLogsFolder()
        {
            return string.Empty;
        }
        internal static void CreateDirectories()
        {
            if (!Directory.Exists(GetLogsFolder()))
                Directory.CreateDirectory(GetLogsFolder());
        }
        internal static string ConstructStringLog(string message, LogLevel logLevel)
        {
            return $"[{logLevel}][{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}]:{message}";
        }
        internal static bool IsUsedByAnotherProcess(string filePath)
        {
            try { using Stream stream = new FileStream(filePath, FileMode.Open); }
            catch { return true; }
            return false;
        }
    }
}