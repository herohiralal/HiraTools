namespace UnityEngine
{
    [System.Serializable]
    public class SerializableBlackboardKey : ScriptableObject
    {
        public SerializableBlackboardKey Setup(string inName, BlackboardKeyType type, bool instanceSynced)
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
}