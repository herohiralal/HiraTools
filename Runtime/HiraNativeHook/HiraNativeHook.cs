using System;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine.Internal
{
    [HiraManager(Priority = byte.MaxValue)]
    public class HiraNativeHook : MonoBehaviour
    {
        private NativeUnityHook _nativeHook = default;
        public static event Action OnNativeHookCreated = delegate { };
        public static event Action OnNativeHookDestroyed = delegate { };
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
            
            OnNativeHookCreated.Invoke();
        }

        private void Update()
        {
            _nativeHook.Update(Time.unscaledDeltaTime, Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _nativeHook.FixedUpdate(Time.fixedUnscaledDeltaTime, Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            _nativeHook.LateUpdate(Time.unscaledDeltaTime, Time.deltaTime);
        }

        private void OnDestroy()
        {
            OnNativeHookDestroyed.Invoke();
            
            _nativeHook.Destroy();
        }

        [SuppressUnmanagedCodeSecurity, DllImport(HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void InitDebugLogToUnity(Action<LogType, string> logger);

        [AOT.MonoPInvokeCallback(typeof(Action<LogType, string>))]
        private static void LogToUnity(LogType type, string message) =>
            Debug.LogFormat(type, LogOption.NoStacktrace, null, $"<color=red><b>Native log: </b></color>{message}");

#if !UNITY_EDITOR
        private void OnGUI()
        {
            if (GUILayout.Button("Quit")) Application.Quit();
        }
#endif
    }
}