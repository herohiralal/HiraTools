using System;
using UnityEngine;

namespace UnityEngine
{
    public abstract class HiraBlackboardKey : ScriptableObject
    {
        [SerializeField] private bool instanceSynced = false;
        [SerializeField] private bool essentialToDecisionMaking = true;
        [NonSerialized] public ushort Index = ushort.MaxValue;

        public BlackboardKeyTraits Traits
        {
            get
            {
                var output = BlackboardKeyTraits.None;
                if (instanceSynced) output |= BlackboardKeyTraits.InstanceSynced;
                if (essentialToDecisionMaking) output |= BlackboardKeyTraits.EssentialToDecisionMaking;
                return output;
            }
        }

        public abstract byte SizeInBytes { get; }
        public abstract unsafe void SetDefault(void* value);
        public abstract unsafe string GetValue(void* data);
    }
}

namespace HiraEngine.Components.Blackboard.Internal
{
    public abstract class BlackboardKey<T> : HiraBlackboardKey where T : unmanaged
    {
        [SerializeField] private T defaultValue = default;

        public sealed override unsafe byte SizeInBytes => (byte) sizeof(T);
        public sealed override unsafe void SetDefault(void* value) => *(T*) value = defaultValue;
        public sealed override unsafe string GetValue(void* data) => (*(T*) data).ToString();
    }

    public interface INumericalBlackboardKey
    {
    }
}