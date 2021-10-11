using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Logging.Events;
using Logging.Handlers;
using Logging.Main;

namespace Logging.Core
{
    public class Log
    {
        private Thread LogThread { get; set; }
        private Dictionary<string, List<string>> Logs { get; set; } = new();
        private string FileName { get; set; } = $"{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}.{DateTime.Now.Hour}.{DateTime.Now.Minute}.{DateTime.Now.Second}.txt";

        internal Log()
        {
            Logs.Add(Info.DefaultLogType, new());
            LogThread = new Thread(LogMethod);
            LogThread.Start();
        }
        ~Log()
        {
            Info.ActiveLog = false;
        }
        #region Helper
        private void LogMethod()
        {
            while (Info.ActiveLog)
            {
                SaveLogs();
                Thread.Sleep(Info.LogThreadSleep);
            }
            SaveLogs();
        }       
        private string GetLogsFullFileName(string logType)
        {
            return Path.Combine(Manage.GetLogsFolder(), logType, FileName);
        }

        private bool SaveLogs(string logType, List<string> logs)
        {
            try
            {
                if (!Directory.Exists(Manage.GetLogsFullPath(logType)))
                    Directory.CreateDirectory(Manage.GetLogsFullPath(logType));
                File.AppendAllLines(GetLogsFullFileName(logType), logs, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                Add($"Catch an exeption {ex.Message} while saving logs", Info.DefaultLogType, LogLevel.Fatal);
                return false;
            }
        }
        #endregion

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
            Action.Main.Manage.Manager.ExecuteEvent<IEventHandlerLog>(new LogEvent(message, logType, logLevel));
            if (!Info.ShouldLog)
                return;
            if (!Logs.ContainsKey(logType))
            {
                if (!Logs.ContainsKey(Info.DefaultLogType))
                {
                    Logs.Add(Info.DefaultLogType, new() { Manage.ConstructStringLog($"{Logs} missed {nameof(Info.DefaultLogType)} - {Info.DefaultLogType}", LogLevel.Fatal) });
                    return;
                }
                Add($"Failed to add log {message} with {nameof(LogLevel)} {logLevel}", Info.DefaultLogType, LogLevel.Error);
                return;
            }
            lock (Logs[logType])
                Logs[logType].Add(Manage.ConstructStringLog(message, logLevel));
        }
        public void SaveLogs()
        {
            if (!Info.ShouldLog)
                return;
            foreach (KeyValuePair<string, List<string>> keyValuePair in Logs)
{
                if (Manage.IsUsedByAnotherProcess(GetLogsFullFileName(keyValuePair.Key)))
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
    }
    public enum LogLevel
    {
        Trace, Debug, Info, Warn, Error, Fatal
    }
}