using System;
using System.Text;

namespace Logging.Main
{
    public class Info
    {
        public const string DefaultLogType = "Application";
        public string FileExtension { get; set; } = "log";
        public bool ActiveLog { get; set; } = true;
        public int LogThreadSleep { get; set; } = 5000;
        public bool ShouldLog { get; set; } = true;
        public string DefaultFolderName { get; set; } = "Logs";
        public string DefaultFolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public string DefaultApplicationName { get; set; } = "TabulaComplex";
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}