using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
    using Internal;

    [HiraManager(Priority = byte.MaxValue)]
    public class NativeHook : MonoBehaviour
    {
        private NativeUnityHook _nativeHook = default;
#if UNITY_EDITOR
        public const string HIRA_ENGINE_NATIVE_DLL_NAME = "HiraEngine-Native-Editor";
#else
        public const string HIRA_ENGINE_NATIVE_DLL_NAME = "HiraEngine-Native";
#endif
        
        private void Awake()
        {
            InitDebugLogToUnity(LogToUnity);
            _nativeHook = NativeUnityHook.Create();
        }

        private void Update()
        {
            _nativeHook.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _nativeHook.Destroy();
        }

        [DllImport(HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void InitDebugLogToUnity(Action<LogType, string> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<LogType, string>))]
        private static void LogToUnity(LogType type, string message) =>
            Debug.LogFormat(type, LogOption.NoStacktrace, null, $"<color=red><b>Native log: </b></color>{message}");

        private void OnGUI()
        {
            if (GUILayout.Button("Quit")) Application.Quit();
        }
    }

    namespace Internal
    {
        internal struct NativeUnityHook
        {
            [DllImport(NativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr CreateUnityHook();

            [DllImport(NativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
            private static extern void DestroyUnityHook(IntPtr target);

            [DllImport(NativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
            private static extern void UnityHookUpdate(IntPtr target, float deltaTime);

            public bool IsValid => _target != IntPtr.Zero;
            private IntPtr _target;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static NativeUnityHook Create() =>
                new NativeUnityHook {_target = CreateUnityHook()};

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Destroy()
            {
                DestroyUnityHook(_target);
                _target = IntPtr.Zero;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Update(float deltaTime) => UnityHookUpdate(_target, deltaTime);
        }
    }
}