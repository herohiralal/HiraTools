using System;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine.Internal
{
    public static class NativeLogger
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void OnLoad()
        {
            HiraNativeHook.PreNativeHookCreated -= Initialize;
            HiraNativeHook.PreNativeHookCreated += Initialize;
        }

        private static void Initialize() => InitLogInternal(Log);

        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private static extern void InitLogInternal(LoggerDelegate logger);

        [UnmanagedFunctionPointer(HiraNativeHook.CALLING_CONVENTION, CharSet = CharSet.Unicode)]
        private delegate void LoggerDelegate(LogType type, IntPtr toLog);

        [AOT.MonoPInvokeCallback(typeof(LoggerDelegate))]
        private static unsafe void Log(LogType type, IntPtr toLog)
        {
            const string prefix = "<color=red><b>Native log: </b></color>";
            Debug.LogFormat(type, LogOption.NoStacktrace, null, $"{prefix}{new string((char*) toLog)}");
        }
    }
}