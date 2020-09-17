using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal
{
    [System.Serializable]
    public class SerializableKey : ScriptableObject
    {
        public SerializableKey Setup(string inName, BlackboardKeyType type, bool instanceSynced)
        {
            (name, keyType, instanceSynchronized) = (new StringReference(inName), type, instanceSynced);
            return this;
        }
        
        public string Name => name;
        
        [SerializeField] private BlackboardKeyType keyType = BlackboardKeyType.Undefined;
        public BlackboardKeyType KeyType => keyType;
        
        [SerializeField] private bool instanceSynchronized = false;
        public bool InstanceSynchronized => instanceSynchronized;

        public uint TypeSpecificIndex { get; set; } = 0;
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