using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal
{
    [System.Serializable]
    public class SerializableKey
    {
        public SerializableKey()
        {
        }

        public SerializableKey(string name, BlackboardKeyType type, bool instanceSynced)
        {
            (keyName, keyType, instanceSynchronized) = (new StringReference(name), type, instanceSynced);
        }
        
        [SerializeField] private StringReference keyName = null;
        internal string Name => keyName.Value;
        
        [SerializeField] private BlackboardKeyType keyType = BlackboardKeyType.Undefined;
        internal BlackboardKeyType KeyType => keyType;
        
        [SerializeField] private bool instanceSynchronized = false;
        internal bool InstanceSynchronized => instanceSynchronized;

        internal uint TypeSpecificIndex { get; set; } = 0;
    }

    public enum BlackboardKeyType
    {
        Undefined,
        Bool,
        Float,
        Int,
        String,
        Vector
    }
}