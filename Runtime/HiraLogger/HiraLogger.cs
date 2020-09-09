using System;
using System.Diagnostics;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace UnityEngine
{
    public static class HiraLogger
    {
        #region Log Message
        
        [Conditional("ENABLE_LOG_MESSAGE")]
        public static void Log(object message) => 
            Debug.Log(message);

        [Conditional("ENABLE_LOG_MESSAGE")]
        public static void Log(object message, Object context) => 
            Debug.Log(message, context);

        [Conditional("ENABLE_LOG_MESSAGE")]
        public static void LogFormat(string format, params object[] args) => 
            Debug.LogFormat(format, args);

        [Conditional("ENABLE_LOG_MESSAGE")]
        public static void LogFormat(Object context, string format, params object[] args) =>
            Debug.LogFormat(context, format, args);

        [Conditional("ENABLE_LOG_MESSAGE")]
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args) =>
            Debug.LogFormat(logType, logOptions, context, format, args);
        
        #endregion
        
        #region Log Error

        [Conditional("ENABLE_LOG_ERROR")]
        public static void LogError(object message) =>
            Debug.LogError(message);

        [Conditional("ENABLE_LOG_ERROR")]
        public static void LogError(object message, Object context) => 
            Debug.LogError(message, context);

        [Conditional("ENABLE_LOG_ERROR")]
        public static void LogErrorFormat(string format, params object[] args) =>
            Debug.LogErrorFormat(format, args);

        [Conditional("ENABLE_LOG_ERROR")]
        public static void LogErrorFormat(Object context, string format, params object[] args) =>
            Debug.LogErrorFormat(context, format, args);
        
        #endregion
        
        #region Log Exception

        [Conditional("ENABLE_LOG_EXCEPTION")]
        public static void LogException(Exception exception) => 
            Debug.LogException(exception);

        [Conditional("ENABLE_LOG_EXCEPTION")]
        public static void LogException(Exception exception, Object context) => 
            Debug.LogException(exception, context);
        
        #endregion
        
        #region Log Warnings

        [Conditional("ENABLE_LOG_WARNING")]
        public static void LogWarning(object message) =>
            Debug.LogWarning(message);

        [Conditional("ENABLE_LOG_WARNING")]
        public static void LogWarning(object message, Object context) =>
            Debug.LogWarning(message, context);

        [Conditional("ENABLE_LOG_WARNING")]
        public static void LogWarningFormat(string format, params object[] args) =>
            Debug.LogWarningFormat(format, args);

        [Conditional("ENABLE_LOG_WARNING")]
        public static void LogWarningFormat(Object context, string format, params object[] args) =>
            Debug.LogWarningFormat(context, format, args);
        
        #endregion
    }
}