using System;
using HiraEngine.Components.Blackboard;

namespace UnityEngine
{
    [CreateAssetMenu(fileName = "New Blackboard Key Set", menuName = "Hiralal/HiraEngine/HiraCreatures/Blackboard Key Set")]
    public class HiraBlackboardKeySet : HiraCollection<SerializableBlackboardKey>, IBlackboardKeyData
    {
        private IBlackboardKeyData _mainKeyData = null;

        private void Awake() => UpdateCache();

        private void OnValidate() => UpdateCache();

        private void UpdateCache() => _mainKeyData = BlackboardTypes.GetKeyData(collection);

        public IReadOnlyInstanceSynchronizer InstanceSynchronizer => _mainKeyData.InstanceSynchronizer;
        public uint GetHash(in string keyName) => _mainKeyData.GetHash(keyName);

        public uint GetTypeSpecificIndex(uint hash) => _mainKeyData.GetTypeSpecificIndex(hash);

        public bool IsInstanceSynchronized(uint hash) => _mainKeyData.IsInstanceSynchronized(hash);

        public IBlackboardValueAccessor ValueAccessor => _mainKeyData.ValueAccessor;

        public SerializableBlackboardKey[] Keys => collection;
    }
}