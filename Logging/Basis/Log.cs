using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Logging.Events;
using Logging.Handlers;
using Logging.Main;

namespace Logging.Basis
{
    public class Log
    {
        private Thread LogThread { get; set; }
        private Dictionary<string, List<string>> Logs { get; set; } = new();
        private string FileName { get; set; } = $"{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}.{DateTime.Now.Hour}.{DateTime.Now.Minute}.{DateTime.Now.Second}";

        internal Log()
        {
            Logs.Add(Info.DefaultLogType, new());
            LogThread = new Thread(LogMethod);
            LogThread.Start();
        }
        ~Log()
        {
            Manage.ManageInstance.Info.ActiveLog = false;
        }

        #region Public
        public bool CreateLogType(string logType)
        {
            if (Logs.ContainsKey(logType))
                return false;
            Logs.Add(logType, new());
            return true;
        }
        public bool DeleteLogType(string logType)
        {
            if (!Logs.ContainsKey(logType))
                return false;
            lock (Logs[logType])
                Logs.Remove(logType);
            return true;
        }
        public void Add(string message, string logType, LogLevel logLevel)
        {
            Action.Main.Manage.ManageInstance.ExecuteEvent<IEventHandlerLog>(new LogEvent(message, logType, logLevel));
            if (!Manage.ManageInstance.Info.ShouldLog)
                return;
            if (!Logs.ContainsKey(logType))
            {
                if (!Logs.ContainsKey(Info.DefaultLogType))
                {
                    Logs.Add(Info.DefaultLogType, new() { ConstructStringLog($"{Logs} missed {nameof(Info.DefaultLogType)} - {Info.DefaultLogType}", LogLevel.Fatal) });
                    return;
                }
                Add($"Failed to add log {message} with {nameof(LogLevel)} {logLevel}", Info.DefaultLogType, LogLevel.Error);
                return;
            }
            lock (Logs[logType])
                Logs[logType].Add(ConstructStringLog(message, logLevel));
        }
        public void Add(string message, LogLevel logLevel)
        {
            Action.Main.Manage.ManageInstance.ExecuteEvent<IEventHandlerLog>(new LogEvent(message, Info.DefaultLogType, logLevel));
            if (!Manage.ManageInstance.Info.ShouldLog)
                return;
            lock (Logs[Info.DefaultLogType])
                Logs[Info.DefaultLogType].Add(ConstructStringLog(message, logLevel));
        }
        public void SaveLogs()
        {
            if (!Manage.ManageInstance.Info.ShouldLog)
                return;
            foreach (KeyValuePair<string, List<string>> keyValuePair in Logs)
{
                if (Manage.ManageInstance.IsUsedByAnotherProcess(GetLogsFullFileName(keyValuePair.Key)))
                    continue;
                List<string> TempLogs = new();
                lock (Logs[keyValuePair.Key])
                    TempLogs.AddRange(Logs[keyValuePair.Key]);
                if (TempLogs.Count > 0)
                {
                    if (SaveLogs(keyValuePair.Key, TempLogs))
                        Logs[keyValuePair.Key].RemoveRange(0, TempLogs.Count);
                }
            }
        }
        #endregion

        #region Helper
        private string GetFullFileName()
        {
            return FileName + "." + Manage.ManageInstance.Info.FileExtension;
        }
        private void LogMethod()
        {
            while (Manage.ManageInstance.Info.ActiveLog)
            {
                SaveLogs();
                Thread.Sleep(Manage.ManageInstance.Info.LogThreadSleep);
            }
            SaveLogs();
        }
        private string GetLogsFullFileName(string logType)
        {
            return Path.Combine(Manage.ManageInstance.GetLogsFolder(), logType, GetFullFileName());
        }
        private string ConstructStringLog(string message, LogLevel logLevel)
        {
            return $"[{logLevel}][{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}]:{message}";
        }
        private bool SaveLogs(string logType, List<string> logs)
        {
            try
            {
                if (!Directory.Exists(Manage.ManageInstance.GetLogsFullPath(logType)))
                    Directory.CreateDirectory(Manage.ManageInstance.GetLogsFullPath(logType));
                File.AppendAllLines(GetLogsFullFileName(logType), logs, Manage.ManageInstance.Info.Encoding);
                return true;
            }
            catch (Exception exception)
            {
                Action.Main.Manage.ManageInstance.ExecuteEvent<IEventHandlerCatchAnException>(new CatchAnExceptionEvent(nameof(SaveLogs), exception, LogLevel.Fatal));
                return false;
            }
        }
        #endregion
    }
    public enum LogLevel
    {
        Trace, Debug, Info, Warn, Error, Fatal
    }
}