using System;
using System.Runtime.InteropServices;
using UnityEngine.Internal;
using UnityEngine.Profiling;

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
#if UNITY_IOS
        public const string HIRA_ENGINE_NATIVE_DLL_NAME = "__Internal";
#else
        public const string HIRA_ENGINE_NATIVE_DLL_NAME = "HiraEngine-Native";
#endif
        public const CallingConvention CALLING_CONVENTION = CallingConvention.StdCall;

        private void Awake()
        {
            InitializeProfilerSamplers();

            using (new ProfilerSampler("NativeHook Pre-Initialization"))
                PreNativeHookCreated?.Invoke();

            var properties = Resources.Load<HiraNativeHookProperties>("HiraNativeHookProperties");

            using (new ProfilerSampler("NativeHook Creation"))
                _nativeHook = NativeUnityHook.Create(properties);

            Resources.UnloadAsset(properties);

            using (new ProfilerSampler("NativeHook Post-Initialization"))
                OnNativeHookCreated?.Invoke();
        }

        private void Update()
        {
#if ENABLE_PROFILER
            _updateSampler.Begin(this);
#endif
            _nativeHook.Update(Time.unscaledDeltaTime, Time.deltaTime);
#if ENABLE_PROFILER
            _updateSampler.End();
#endif
        }

        private void FixedUpdate()
        {
#if ENABLE_PROFILER
            _fixedUpdateSampler.Begin();
#endif
            _nativeHook.FixedUpdate(Time.fixedUnscaledDeltaTime, Time.fixedDeltaTime);
#if ENABLE_PROFILER
            _fixedUpdateSampler.End();
#endif
        }

        private void LateUpdate()
        {
#if ENABLE_PROFILER
            _lateUpdateSampler.Begin();
#endif
            _nativeHook.LateUpdate(Time.unscaledDeltaTime, Time.deltaTime);
#if ENABLE_PROFILER
            _lateUpdateSampler.End();
#endif
        }

        private void OnDestroy()
        {
            using (new ProfilerSampler("NativeHook Pre-Shutdown"))
                OnNativeHookDestroyed?.Invoke();

            using (new ProfilerSampler("NativeHook Pre-Shutdown"))
                _nativeHook.Destroy();

            using (new ProfilerSampler("NativeHook Pre-Shutdown"))
                PostNativeHookDestroyed?.Invoke();
        }


#if ENABLE_PROFILER
        private CustomSampler _updateSampler;
        private CustomSampler _fixedUpdateSampler;
        private CustomSampler _lateUpdateSampler;
#endif
        private void InitializeProfilerSamplers()
        {
#if ENABLE_PROFILER
            _updateSampler = CustomSampler.Create("NativeHook Update");
            _fixedUpdateSampler = CustomSampler.Create("NativeHook FixedUpdate");
            _lateUpdateSampler = CustomSampler.Create("NativeHook LateUpdate");
#endif
        }

        private readonly struct ProfilerSampler : IDisposable
        {
            public ProfilerSampler(string name)
            {
#if ENABLE_PROFILER
                _sampler = CustomSampler.Create(name);
                _sampler.Begin();
#endif
            }

#if ENABLE_PROFILER
            private readonly CustomSampler _sampler;
#endif

            public void Dispose()
            {
#if ENABLE_PROFILER
                _sampler.End();
#endif
            }
        }
    }
}