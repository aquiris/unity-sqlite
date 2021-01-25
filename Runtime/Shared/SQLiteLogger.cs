using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Aquiris.SQLite.Shared
{
    public enum LoggingLevel
    {
        [UsedImplicitly] Off,
        [UsedImplicitly] Error,
        [UsedImplicitly] Info,
        [UsedImplicitly] Verbose,
    }
    
    public static class SQLiteLogger
    {
        private static readonly StringBuilder _logBuilder = new StringBuilder(); 
        
        [UsedImplicitly]
        public static LoggingLevel loggingLevel { get; set; }
        
        [UsedImplicitly, Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
        internal static void Log(object message)
        {
            if (loggingLevel < LoggingLevel.Verbose) return;
            Debug.Log(message);
        }

        [UsedImplicitly, Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
        internal static void Log(params LogPart[] parts)
        {
            if (loggingLevel < LoggingLevel.Verbose) return;
            Log(PrepareMessage(parts));
        }
        
        [UsedImplicitly, Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
        internal static void LogWarning(object message)
        {
            if (loggingLevel < LoggingLevel.Info) return;
            Debug.LogWarning(message);
        }

        [UsedImplicitly, Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
        internal static void LogWarning(params LogPart[] parts)
        {
            if (loggingLevel < LoggingLevel.Info) return;
            LogWarning(PrepareMessage(parts));
        }

        [UsedImplicitly, Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
        internal static void LogError(object message)
        {
            if (loggingLevel < LoggingLevel.Error) return;
            Debug.LogError(message);
        }

        [UsedImplicitly, Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
        internal static void LogError(params LogPart[] parts)
        {
            if (loggingLevel < LoggingLevel.Error) return;
            LogError(PrepareMessage(parts));
        }
        
#if UNITY_EDITOR || DEBUG
        private static string PrepareMessage(LogPart[] parts)
        {
            _logBuilder.Clear();
            for (int index = 0; index < parts.Length; index++)
            {
                LogPart part = parts[index];
                Color c = part.color;
                _logBuilder.AppendFormat("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
                    LogPart.GetHex(c.r),
                    LogPart.GetHex(c.g),
                    LogPart.GetHex(c.b),
                    part.message);
            }
            return _logBuilder.ToString();
        }

        internal readonly struct LogPart
        {
            public static LogPart newLine = new LogPart("\n", Color.white);
            
            public readonly object message;
            public readonly Color color;

            public LogPart(object message, Color color)
            {
                this.message = message;
                this.color = color;
            }

            internal static byte GetHex(float value)
            {
                return (byte) (value * 255F);
            }
        }
#endif
    }
}
