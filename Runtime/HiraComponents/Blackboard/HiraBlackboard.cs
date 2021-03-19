using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine
{
    public class HiraBlackboard : MonoBehaviour, IInitializable, IBlackboardComponent
    {
        [SerializeField] public HiraBlackboardTemplate template = null;
        [NonSerialized] public NativeArray<byte> Data = default;
        [SerializeField] public bool broadcastKeyUpdateEvents = true;

        public event Action OnKeyEssentialToDecisionMakingUpdate = delegate { };

        public unsafe void Initialize<T>(ref T initParams)
        {
            Data = template.GetNewBlackboard();
            template.OnInstanceSyncKeyUpdate += OnInstanceSyncedValueUpdate;
        }

        public unsafe void Shutdown()
        {
            template.OnInstanceSyncKeyUpdate -= OnInstanceSyncedValueUpdate;
            Data.Dispose();
        }

        public T GetValue<T>(string keyName) where T : unmanaged =>
            GetValue<T>(template[keyName]);

        public unsafe T GetValue<T>(ushort keyIndex) where T : unmanaged =>
            *(T*) ((byte*) Data.GetUnsafeReadOnlyPtr() + keyIndex);

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
                *(T*) ((byte*) Data.GetUnsafePtr() + keyIndex) = value;

                if (broadcastKeyUpdateEvents && traits.HasFlag(BlackboardKeyTraits.EssentialToDecisionMaking))
                    OnKeyEssentialToDecisionMakingUpdate.Invoke();
            }
        }

        private unsafe void OnInstanceSyncedValueUpdate(ushort keyIndex, bool isEssentialToDecisionMaking, byte* value, byte size)
        {
            var key = (byte*) Data.GetUnsafePtr() + keyIndex;
            for (byte i = 0; i < size; i++) key[i] = value[i];

            if (broadcastKeyUpdateEvents && isEssentialToDecisionMaking)
                OnKeyEssentialToDecisionMakingUpdate.Invoke();
        }
    }
}