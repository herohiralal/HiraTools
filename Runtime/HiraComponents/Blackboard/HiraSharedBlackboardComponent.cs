﻿using System;
using Unity.Collections;

namespace UnityEngine
{
    public class HiraSharedBlackboardComponent : MonoBehaviour, IBlackboardComponent
    {
        [SerializeField] private HiraSharedBlackboard sharedBlackboard = null;
        [SerializeField] private HiraBlackboardCore core = null;
        private void Awake() => core = sharedBlackboard.core;
        private void OnDestroy() => core = null;
        public NativeArray<byte> Data => core.Data;

        public event Action OnKeyEssentialToDecisionMakingUpdate
        {
            add => core.OnKeyEssentialToDecisionMakingUpdate += value;
            remove => core.OnKeyEssentialToDecisionMakingUpdate -= value;
        }

        public T GetValue<T>(string keyName) where T : unmanaged => core.GetValue<T>(keyName);
        public T GetValue<T>(ushort keyIndex) where T : unmanaged => core.GetValue<T>(keyIndex);
        public void SetValue<T>(string keyName, T value) where T : unmanaged => core.SetValue(keyName, value);
        public void SetValue<T>(ushort keyIndex, T value) where T : unmanaged => core.SetValue(keyIndex, value);
    }
}