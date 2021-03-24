using System;
using Unity.Collections;

namespace UnityEngine
{
    public class HiraBlackboardComponent : MonoBehaviour, IInitializable, IBlackboardComponent
    {
        [SerializeField] private HiraBlackboardCore core = null;
        public HiraBlackboardTemplate Template => core.template;
        public void Initialize<T>(ref T initParams) => core.Initialize(ref initParams);
        public void Shutdown() => core.Shutdown();
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