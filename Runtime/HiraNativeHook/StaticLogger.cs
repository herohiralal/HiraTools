// ReSharper disable All
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine.Internal
{
    public static class StaticLogger
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void OnLoad()
        {
            HiraNativeHook.PreNativeHookCreated -= Initialize;
            HiraNativeHook.PreNativeHookCreated += Initialize;
        }

        private static void Initialize()
        {
            InitLoggerLogStart(LogStart);
            InitIntegerLogger(LogInteger);
            InitStringLogger(LogString);
            InitFloatLogger(LogFloat);
            InitLoggerLogEnd(LogEnd);
        }

        private const string prefix = "<color=red><b>Native log: </b></color>";
        private static LogType _trackedLogType = LogType.Log;
        private static readonly System.Text.StringBuilder string_builder = new System.Text.StringBuilder(1000);

        // Start Logger
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitLoggerLogStart(Action<LogType> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<LogType>))]
        private static void LogStart(LogType type)
        {
            _trackedLogType = type;
            string_builder.Clear();
            string_builder.Append(prefix);
        }

        // End Logger
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitLoggerLogEnd(Action logger);

        [AOT.MonoPInvokeCallback(typeof(Action))]
        private static void LogEnd()
        {
            Debug.LogFormat(_trackedLogType, LogOption.NoStacktrace, null, string_builder.ToString());
        }

        // Integer
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitIntegerLogger(Action<int> logger);

        private static void LogInteger(int toLog) => string_builder.Append(toLog.ToString());

        // String
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitStringLogger(Action<string> logger);

        private static void LogString(string toLog) => string_builder.Append(toLog.ToString());

        // Float
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitFloatLogger(Action<float> logger);

        private static void LogFloat(float toLog) => string_builder.Append(toLog.ToString());
    }
}
