using System;

namespace Logging.Main
{
    public static class Info
    {
        public static int LogThreadSleep { get; set; } = 5000;
        internal static bool ActiveLog { get; set; } = true;
        public static bool ShouldLog { get; set; } = true;
        public static string DefaultFolderName { get; set; } = "Logs";
        public static string DefaultFolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string DefaultLogType { get; set; } = "Application";
    }
}