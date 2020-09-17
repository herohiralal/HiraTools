using System.Linq;
using HiraCreatures.Components.Blackboard;
using HiraCreatures.Components.Blackboard.Internal;
using HiraCreatures.Components.Blackboard.Helpers;

namespace UnityEngine
{
    [CreateAssetMenu(fileName = "New Blackboard Key Set", menuName = "Hiralal/HiraEngine/HiraCreatures/Blackboard Key Set")]
    public class HiraBlackboardKeySet : HiraCollection<SerializableKey>, IBlackboardKeyData
    {
        private IBlackboardKeyData _mainKeyData = null;
        
        public void Activate()
        {
            _mainKeyData = BlackboardTypes.GetKeyData(collection);
            _mainKeyData.Activate();
        }

        public void Deactivate()
        {
            _mainKeyData.Deactivate();
            _mainKeyData = null;
        }

        public IReadOnlyInstanceSynchronizer InstanceSynchronizer => _mainKeyData.InstanceSynchronizer;
        public uint GetHash(in string keyName) => _mainKeyData.GetHash(keyName);

        public uint GetTypeSpecificIndex(uint hash) => _mainKeyData.GetTypeSpecificIndex(hash);

        public bool IsInstanceSynchronized(uint hash) => _mainKeyData.IsInstanceSynchronized(hash);

        public IBlackboardValueAccessor ValueAccessor => _mainKeyData.ValueAccessor;

        public SerializableKey[] Keys => collection;
    }
}