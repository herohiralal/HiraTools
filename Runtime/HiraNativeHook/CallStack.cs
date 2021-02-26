using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Internal
{
    internal static class CallStackMaintainer
    {
        [DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern IntPtr CallStackInitialize(ushort maxStackSizeHint);

        [DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void CallStackShutdown();

        [DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void InitCheckCallStack(Action checker);

        [DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void TestCallStack();

        private static IntPtr _stackData = IntPtr.Zero;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnLoad()
        {
            HiraNativeHook.PreNativeHookCreated -= Initialize;
            HiraNativeHook.PreNativeHookCreated += Initialize;
        }

        private static void Initialize()
        {
            HiraNativeHook.PreNativeHookCreated -= Initialize;
            
            _stackData = CallStackInitialize(8);
            InitCheckCallStack(CheckCallStack);

            HiraNativeHook.PostNativeHookDestroyed -= Shutdown;
            HiraNativeHook.PostNativeHookDestroyed += Shutdown;
        }

        private static void Shutdown()
        {
            HiraNativeHook.PostNativeHookDestroyed -= Shutdown;
            
            CallStackShutdown();
            _stackData = IntPtr.Zero;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("HiraNativeHook/Test Call-Stack")]
        private static void TestCallStackInternal() => TestCallStack();
#endif

        private static unsafe void CheckCallStack()
        {
            var data = (ushort*) _stackData;
            var stackCount = *data;
            if (stackCount == 0)
            {
                Debug.Log("Stack empty.");
                return;
            }

            var s = "";

            var it = data + 1;
            for (var i = 0; i < stackCount; i++)
            {
                var functionNameSize = it[0];
                var fileNameSize = it[1];
                var lineNumber = it[2];

                var functionName = new string((char*) (it + 3));
                var fileName = new string((char*) (it + 3 + functionNameSize));

                s += $"(size: {functionNameSize}){functionName}() (at (size: {fileNameSize}){fileName}:{lineNumber})\n";
                it += 0
                      + 1 // function-name size
                      + 1 // file-name size
                      + 1 // line number
                      + functionNameSize // function-name
                      + fileNameSize; // file-name
            }

            Debug.Log(s);
        }
    }
}