using System;
using Unity.Collections;

namespace UnityEngine
{
    [CreateAssetMenu(menuName = "Hiralal/AI/Shared Blackboard", fileName = "New Shared Blackboard")]
    public class HiraSharedBlackboard : ScriptableObject, IInitializable, IBlackboardComponent
    {
        [SerializeField] public HiraBlackboardCore core = null;
        public HiraBlackboardTemplate Template => core.template;
        public void Initialize() => core.Initialize();
        public void Shutdown() => core.Shutdown();
        public NativeArray<byte> Data => core.Data;

        public bool BroadcastKeyUpdateEvents
        {
            get => core.BroadcastKeyUpdateEvents;
            set => core.BroadcastKeyUpdateEvents = value;
        }

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