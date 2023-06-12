using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Asteroids.Tools
{
    public enum LogCategory
    {
        Common
    }

    public enum LogType
    {
        Assertion,
        Debug,
        Warning,
        Error,
    }

    [Flags]
    public enum LogOutput
    {
        None,
        Console,
        File,
    }

    public static class Logger
    {
        private static readonly string PersistentDataPath = Application.persistentDataPath;
        private const string LogFileName = "Log.txt";
        private static readonly string LogPath = Path.Combine(PersistentDataPath + LogFileName);
        private static StreamWriter _fileWriter;

        private static void InitializeFileWriter()
        {
            if (_fileWriter != null)
            {
                _fileWriter.Dispose();
            }

            _fileWriter = new StreamWriter(LogPath, true);
        }

        private static StreamWriter FileWriter
        {
            get
            {
                if (_fileWriter == null)
                {
                    InitializeFileWriter();
                }

                return _fileWriter;
            }
        }

        public static LogOutput DefaultAssertionLogOutput = LogOutput.None;
        public static LogOutput DefaultDebugLogOutput = LogOutput.Console;
        public static LogOutput DefaultWarningLogOutput = LogOutput.Console | LogOutput.File;
        public static LogOutput DefaultErrorLogOutput = LogOutput.Console | LogOutput.File;

        public static readonly Dictionary<LogCategory, Dictionary<LogType, LogOutput>> OutputSettings = new Dictionary<LogCategory, Dictionary<LogType, LogOutput>>()
        {
            {
                LogCategory.Common, new Dictionary<LogType, LogOutput>()
                {
                    { LogType.Assertion, DefaultAssertionLogOutput },
                    { LogType.Debug, DefaultDebugLogOutput },
                    { LogType.Warning, DefaultWarningLogOutput },
                    { LogType.Error, DefaultErrorLogOutput },
                }
            }
        };

        public static void Log(this object invoker, LogCategory logCategory, string data)
        {
            if (LogEnabled(LogType.Debug, logCategory))
            {
                LogData(invoker, LogType.Debug, logCategory, data);
            }
        }

        public static void LogWarning(this object invoker, LogCategory logCategory, string data)
        {
            if (LogEnabled(LogType.Warning, logCategory))
            {
                LogData(invoker, LogType.Warning, logCategory, data);
            }
        }

        public static void LogError(this object invoker, LogCategory logCategory, string data)
        {
            if (LogEnabled(LogType.Error, logCategory))
            {
                LogData(invoker, LogType.Error, logCategory, data);
            }
        }

        public static void LogAssertion(this object invoker, LogCategory logCategory, string data)
        {
            if (LogEnabled(LogType.Assertion, logCategory))
            {
                LogData(invoker, LogType.Assertion, logCategory, data);
            }
        }

        private static bool LogEnabled(LogType logType, LogCategory logCategory)
        {
            return OutputSettings[logCategory][logType].HasFlag(LogOutput.File) || OutputSettings[logCategory][logType].HasFlag(LogOutput.Console);
        }

        private static void LogData(object invoker, LogType logType, LogCategory logCategory, string data)
        {
            var message = FormatMessage(invoker, logCategory, data);

            if (OutputSettings[logCategory][logType].HasFlag(LogOutput.Console))
            {
                LogToConsole(logType, message);
            }
            if (OutputSettings[logCategory][logType].HasFlag(LogOutput.File))
            {
                LogToFile(logType, message);
            }
        }

        private static void LogToConsole(LogType logType, string data)
        {
            switch (logType)
            {
                case LogType.Assertion:
                {
                    Debug.LogAssertion(data);
                    break;
                }
                case LogType.Debug:
                {
                    Debug.Log(data);
                    break;
                }
                case LogType.Warning:
                {
                    Debug.LogWarning(data);
                    break;
                }
                case LogType.Error:
                {
                    Debug.LogError(data);
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static void LogToFile(LogType logType, string data)
        {
            try
            {
                FileWriter.Write($"\n{logType} {data}");
                FileWriter.Flush();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                InitializeFileWriter();
            }
        }

        private static string FormatMessage(object invoker, LogCategory logCategory, string message)
        {
            var typeFullName = invoker.GetType().FullName;
            var invokerComponent = invoker as Component;

            string componentName = null;
            if ((object) invokerComponent != null)
            {
                componentName = invokerComponent != null ? invokerComponent.name : "Destroyed";
            }

            return $"{DateTime.Now} {logCategory} {typeFullName} {componentName} : {message}";
        }
    }
}
