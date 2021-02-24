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
            InitStringLogger(LogString);
            InitSignedByteLogger(LogSignedByte);
            InitByteLogger(LogByte);
            InitShortLogger(LogShort);
            InitUnsignedShortLogger(LogUnsignedShort);
            InitIntegerLogger(LogInteger);
            InitUnsignedIntegerLogger(LogUnsignedInteger);
            InitLongLogger(LogLong);
            InitUnsignedLongLogger(LogUnsignedLong);
            InitFloatLogger(LogFloat);
            InitDoubleLogger(LogDouble);
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

        // String
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitStringLogger(Action<string> logger);

        private static void LogString(string toLog) => string_builder.Append(toLog.ToString());

        // SignedByte
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitSignedByteLogger(Action<sbyte> logger);

        private static void LogSignedByte(sbyte toLog) => string_builder.Append(toLog.ToString());

        // Byte
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitByteLogger(Action<byte> logger);

        private static void LogByte(byte toLog) => string_builder.Append(toLog.ToString());

        // Short
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitShortLogger(Action<short> logger);

        private static void LogShort(short toLog) => string_builder.Append(toLog.ToString());

        // UnsignedShort
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitUnsignedShortLogger(Action<ushort> logger);

        private static void LogUnsignedShort(ushort toLog) => string_builder.Append(toLog.ToString());

        // Integer
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitIntegerLogger(Action<int> logger);

        private static void LogInteger(int toLog) => string_builder.Append(toLog.ToString());

        // UnsignedInteger
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitUnsignedIntegerLogger(Action<uint> logger);

        private static void LogUnsignedInteger(uint toLog) => string_builder.Append(toLog.ToString());

        // Long
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitLongLogger(Action<long> logger);

        private static void LogLong(long toLog) => string_builder.Append(toLog.ToString());

        // UnsignedLong
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitUnsignedLongLogger(Action<ulong> logger);

        private static void LogUnsignedLong(ulong toLog) => string_builder.Append(toLog.ToString());

        // Float
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitFloatLogger(Action<float> logger);

        private static void LogFloat(float toLog) => string_builder.Append(toLog.ToString());

        // Double
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitDoubleLogger(Action<double> logger);

        private static void LogDouble(double toLog) => string_builder.Append(toLog.ToString());
    }
}
