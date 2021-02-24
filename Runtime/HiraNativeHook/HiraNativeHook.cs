using System;
using System.Runtime.InteropServices;
using UnityEngine.Internal;

namespace UnityEngine
{
    [HiraManager(Priority = byte.MaxValue)]
    public class HiraNativeHook : MonoBehaviour
    {
        private NativeUnityHook _nativeHook = default;
        public static event Action PreNativeHookCreated;
        public static event Action OnNativeHookCreated;
        public static event Action OnNativeHookDestroyed;
        public static event Action PostNativeHookDestroyed;
#if UNITY_EDITOR
        public const string HIRA_ENGINE_NATIVE_DLL_NAME = "HiraEngine-Native-Editor";
#else
        public const string HIRA_ENGINE_NATIVE_DLL_NAME = "HiraEngine-Native";
#endif
        public const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

        private void Awake()
        {
            PreNativeHookCreated?.Invoke();

            var properties = Resources.Load<HiraNativeHookProperties>("HiraNativeHookProperties");
            _nativeHook = NativeUnityHook.Create(properties);
            Resources.UnloadAsset(properties);
            
            OnNativeHookCreated?.Invoke();
        }

        private void Update() => _nativeHook.Update(Time.unscaledDeltaTime, Time.deltaTime);

        private void FixedUpdate() => _nativeHook.FixedUpdate(Time.fixedUnscaledDeltaTime, Time.fixedDeltaTime);

        private void LateUpdate() => _nativeHook.LateUpdate(Time.unscaledDeltaTime, Time.deltaTime);

        private void OnDestroy()
        {
            OnNativeHookDestroyed?.Invoke();
            
            _nativeHook.Destroy();
            
            PostNativeHookDestroyed?.Invoke();
        }
    }
}