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
            InitBooleanLogger(LogBoolean);
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

        // Boolean
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitBooleanLogger(Action<byte> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<byte>))]
        private static void LogBoolean(byte toLog) => string_builder.Append((toLog != 0).ToString());

        // String
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitStringLogger(Action<string> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<string>))]
        private static void LogString(string toLog) => string_builder.Append(toLog.ToString());

        // SignedByte
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitSignedByteLogger(Action<sbyte> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<sbyte>))]
        private static void LogSignedByte(sbyte toLog) => string_builder.Append(toLog.ToString());

        // Byte
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitByteLogger(Action<byte> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<byte>))]
        private static void LogByte(byte toLog) => string_builder.Append(toLog.ToString());

        // Short
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitShortLogger(Action<short> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<short>))]
        private static void LogShort(short toLog) => string_builder.Append(toLog.ToString());

        // UnsignedShort
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitUnsignedShortLogger(Action<ushort> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<ushort>))]
        private static void LogUnsignedShort(ushort toLog) => string_builder.Append(toLog.ToString());

        // Integer
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitIntegerLogger(Action<int> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<int>))]
        private static void LogInteger(int toLog) => string_builder.Append(toLog.ToString());

        // UnsignedInteger
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitUnsignedIntegerLogger(Action<uint> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<uint>))]
        private static void LogUnsignedInteger(uint toLog) => string_builder.Append(toLog.ToString());

        // Long
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitLongLogger(Action<long> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<long>))]
        private static void LogLong(long toLog) => string_builder.Append(toLog.ToString());

        // UnsignedLong
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitUnsignedLongLogger(Action<ulong> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<ulong>))]
        private static void LogUnsignedLong(ulong toLog) => string_builder.Append(toLog.ToString());

        // Float
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitFloatLogger(Action<float> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<float>))]
        private static void LogFloat(float toLog) => string_builder.Append(toLog.ToString());

        // Double
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitDoubleLogger(Action<double> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<double>))]
        private static void LogDouble(double toLog) => string_builder.Append(toLog.ToString());
    }
}
