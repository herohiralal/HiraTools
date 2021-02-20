﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Internal
{
    internal struct NativeUnityHook
    {
        [DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateUnityHook(HiraNativeHookInitParams initParams);

        [DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DestroyUnityHook(IntPtr target);

        [DllImport(HiraNativeHook.HIRA_ENGINE_NATIVE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void UnityHookUpdate(IntPtr target, float deltaTime);

        public bool IsValid => _target != IntPtr.Zero;
        private IntPtr _target;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeUnityHook Create(HiraNativeHookInitParams initParams) =>
            new NativeUnityHook {_target = CreateUnityHook(initParams)};

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