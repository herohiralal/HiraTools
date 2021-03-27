using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine
{
    [Serializable]
    public class HiraBlackboardCore : IInitializable, IBlackboardComponent
    {
        [SerializeField] public HiraBlackboardTemplate template = null;
        [NonSerialized] private NativeArray<byte> _data = default;
        [SerializeField] public bool broadcastKeyUpdateEvents = true;

        public HiraBlackboardTemplate Template => template;

        public NativeArray<byte> Data => _data;

        public bool BroadcastKeyUpdateEvents
        {
            get => broadcastKeyUpdateEvents;
            set => broadcastKeyUpdateEvents = value;
        }

        public event Action OnKeyEssentialToDecisionMakingUpdate = delegate { };

        public unsafe void Initialize()
        {
            _data = template.GetNewBlackboard();
            template.OnInstanceSyncKeyUpdate += OnInstanceSyncedValueUpdate;
        }

        public unsafe void Shutdown()
        {
            template.OnInstanceSyncKeyUpdate -= OnInstanceSyncedValueUpdate;
            _data.Dispose();
        }

        public T GetValue<T>(string keyName) where T : unmanaged =>
            GetValue<T>(template[keyName]);

        public unsafe T GetValue<T>(ushort keyIndex) where T : unmanaged =>
            *(T*) ((byte*) _data.GetUnsafeReadOnlyPtr() + keyIndex);

        public void SetValue<T>(string keyName, T value) where T : unmanaged =>
            SetValue<T>(template[keyName], value);

        public unsafe void SetValue<T>(ushort keyIndex, T value) where T : unmanaged
        {
            var traits = template[keyIndex];
            if (traits.HasFlag(BlackboardKeyTraits.InstanceSynced))
            {
                template.UpdateInstanceSyncedKey(keyIndex, value);
            }
            else
            {
                *(T*) ((byte*) _data.GetUnsafePtr() + keyIndex) = value;

                if (broadcastKeyUpdateEvents && traits.HasFlag(BlackboardKeyTraits.EssentialToDecisionMaking))
                    OnKeyEssentialToDecisionMakingUpdate.Invoke();
            }
        }

        private unsafe void OnInstanceSyncedValueUpdate(ushort keyIndex, bool isEssentialToDecisionMaking, byte* value, byte size)
        {
            var key = (byte*) _data.GetUnsafePtr() + keyIndex;
            for (byte i = 0; i < size; i++) key[i] = value[i];

            if (broadcastKeyUpdateEvents && isEssentialToDecisionMaking)
                OnKeyEssentialToDecisionMakingUpdate.Invoke();
        }
    }
}