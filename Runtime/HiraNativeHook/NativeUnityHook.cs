using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine.Internal
{
    internal struct NativeUnityHook
    {
        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern IntPtr CreateUnityHook([In] ref HiraNativeHookInitParams initParams);

        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void UnityHookDispose(IntPtr target);

        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void DestroyUnityHook(IntPtr target);

        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void UnityHookUpdate(IntPtr target, float unscaledDeltaTime, float deltaTime);

        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void UnityHookFixedUpdate(IntPtr target, float fixedUnscaledDeltaTime, float fixedDeltaTime);

        [SuppressUnmanagedCodeSecurity, DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = HiraNativeHook.CALLING_CONVENTION)]
        private static extern void UnityHookLateUpdate(IntPtr target, float unscaledDeltaTime, float deltaTime);

        public bool IsValid => _target != IntPtr.Zero;
        private IntPtr _target;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeUnityHook Create(HiraNativeHookInitParams initParams) =>
            new NativeUnityHook {_target = CreateUnityHook(ref initParams)};

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            UnityHookDispose(_target);
            DestroyUnityHook(_target);
            _target = IntPtr.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(float unscaledDeltaTime, float deltaTime) => UnityHookUpdate(_target, unscaledDeltaTime, deltaTime);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FixedUpdate(float fixedUnscaledDeltaTime, float fixedDeltaTime) => UnityHookFixedUpdate(_target, fixedUnscaledDeltaTime, fixedDeltaTime);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LateUpdate(float unscaledDeltaTime, float deltaTime) => UnityHookLateUpdate(_target, unscaledDeltaTime, deltaTime);
    }
}