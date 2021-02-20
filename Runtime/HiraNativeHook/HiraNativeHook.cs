using System;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine.Internal
{
    [HiraManager(Priority = byte.MaxValue)]
    public class HiraNativeHook : MonoBehaviour
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

            var properties = Resources.Load<HiraNativeHookProperties>("HiraNativeHookProperties");
            _nativeHook = NativeUnityHook.Create(properties);
            Resources.UnloadAsset(properties);
        }

        private void Update()
        {
            _nativeHook.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _nativeHook.Destroy();
        }

        [SuppressUnmanagedCodeSecurity, DllImport(HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void InitDebugLogToUnity(Action<LogType, string> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<LogType, string>))]
        private static void LogToUnity(LogType type, string message) =>
            Debug.LogFormat(type, LogOption.NoStacktrace, null, $"<color=red><b>Native log: </b></color>{message}");

        private void OnGUI()
        {
            if (GUILayout.Button("Quit")) Application.Quit();
        }
    }
}