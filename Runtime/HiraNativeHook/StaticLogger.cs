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
            unsafe { InitWideStringLogger(LogWideString); }
            InitBooleanLogger(LogBoolean);
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
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitLoggerLogStart(Action<LogType> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<LogType>))]
        private static void LogStart(LogType type)
        {
            _trackedLogType = type;
            string_builder.Clear();
            string_builder.Append(prefix);
        }

        // End Logger
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitLoggerLogEnd(Action logger);

        [AOT.MonoPInvokeCallback(typeof(Action))]
        private static void LogEnd()
        {
            Debug.LogFormat(_trackedLogType, LogOption.NoStacktrace, null, string_builder.ToString());
        }

        // WideString
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitWideStringLogger(LogWideStringDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private unsafe delegate void LogWideStringDelegate(char* x);

        [AOT.MonoPInvokeCallback(typeof(LogWideStringDelegate))]
        private static unsafe void LogWideString(char* toLog) => string_builder.Append(new string(toLog));

        // Boolean
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitBooleanLogger(LogBooleanDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogBooleanDelegate(byte toLog);

        [AOT.MonoPInvokeCallback(typeof(LogBooleanDelegate))]
        private static void LogBoolean(byte toLog) => string_builder.Append((toLog != 0).ToString());

        // SignedByte
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitSignedByteLogger(LogSignedByteDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogSignedByteDelegate(sbyte toLog);

        [AOT.MonoPInvokeCallback(typeof(LogSignedByteDelegate))]
        private static void LogSignedByte(sbyte toLog) => string_builder.Append(toLog.ToString());

        // Byte
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitByteLogger(LogByteDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogByteDelegate(byte toLog);

        [AOT.MonoPInvokeCallback(typeof(LogByteDelegate))]
        private static void LogByte(byte toLog) => string_builder.Append(toLog.ToString());

        // Short
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitShortLogger(LogShortDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogShortDelegate(short toLog);

        [AOT.MonoPInvokeCallback(typeof(LogShortDelegate))]
        private static void LogShort(short toLog) => string_builder.Append(toLog.ToString());

        // UnsignedShort
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitUnsignedShortLogger(LogUnsignedShortDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogUnsignedShortDelegate(ushort toLog);

        [AOT.MonoPInvokeCallback(typeof(LogUnsignedShortDelegate))]
        private static void LogUnsignedShort(ushort toLog) => string_builder.Append(toLog.ToString());

        // Integer
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitIntegerLogger(LogIntegerDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogIntegerDelegate(int toLog);

        [AOT.MonoPInvokeCallback(typeof(LogIntegerDelegate))]
        private static void LogInteger(int toLog) => string_builder.Append(toLog.ToString());

        // UnsignedInteger
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitUnsignedIntegerLogger(LogUnsignedIntegerDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogUnsignedIntegerDelegate(uint toLog);

        [AOT.MonoPInvokeCallback(typeof(LogUnsignedIntegerDelegate))]
        private static void LogUnsignedInteger(uint toLog) => string_builder.Append(toLog.ToString());

        // Long
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitLongLogger(LogLongDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogLongDelegate(long toLog);

        [AOT.MonoPInvokeCallback(typeof(LogLongDelegate))]
        private static void LogLong(long toLog) => string_builder.Append(toLog.ToString());

        // UnsignedLong
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitUnsignedLongLogger(LogUnsignedLongDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogUnsignedLongDelegate(ulong toLog);

        [AOT.MonoPInvokeCallback(typeof(LogUnsignedLongDelegate))]
        private static void LogUnsignedLong(ulong toLog) => string_builder.Append(toLog.ToString());

        // Float
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitFloatLogger(LogFloatDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogFloatDelegate(float toLog);

        [AOT.MonoPInvokeCallback(typeof(LogFloatDelegate))]
        private static void LogFloat(float toLog) => string_builder.Append(toLog.ToString());

        // Double
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitDoubleLogger(LogDoubleDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LogDoubleDelegate(double toLog);

        [AOT.MonoPInvokeCallback(typeof(LogDoubleDelegate))]
        private static void LogDouble(double toLog) => string_builder.Append(toLog.ToString());
    }
}
