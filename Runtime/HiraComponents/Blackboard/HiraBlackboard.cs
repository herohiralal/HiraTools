namespace UnityEngine
{
    public abstract class HiraBlackboard : MonoBehaviour, IBlackboardComponent
    {
        protected abstract HiraBlackboardCore Core { get; }
        public HiraBlackboardTemplate Template => Core.template;
        public Unity.Collections.NativeArray<byte> Data => Core.Data;

        public bool BroadcastKeyUpdateEvents
        {
            get => Core.BroadcastKeyUpdateEvents;
            set => Core.BroadcastKeyUpdateEvents = value;
        }

        public event System.Action OnKeyEssentialToDecisionMakingUpdate
        {
            add => Core.OnKeyEssentialToDecisionMakingUpdate += value;
            remove => Core.OnKeyEssentialToDecisionMakingUpdate -= value;
        }

        public T GetValue<T>(string keyName) where T : unmanaged => Core.GetValue<T>(keyName);
        public T GetValue<T>(ushort keyIndex) where T : unmanaged => Core.GetValue<T>(keyIndex);
        public void SetValue<T>(string keyName, T value) where T : unmanaged => Core.SetValue(keyName, value);
        public void SetValue<T>(ushort keyIndex, T value) where T : unmanaged => Core.SetValue(keyIndex, value);
    }
}