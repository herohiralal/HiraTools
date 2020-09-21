using HiraEngine.Components.Blackboard;
using HiraEngine.Components.Blackboard.Internal;

namespace UnityEngine
{
    public static class BlackboardTypes
    {
        public static IReadWriteBlackboardDataSet GetWriteableDataSet(uint boolKeyCount = default,
            uint floatKeyCount = default,
            uint intKeyCount = default,
            uint stringKeyCount = default,
            uint vectorKeyCount = default) =>
            new DataSet(boolKeyCount, floatKeyCount, intKeyCount, stringKeyCount, vectorKeyCount);

        public static IInstanceSynchronizer GetSynchronizer() =>
            new InstanceSynchronizer();

        public static IBlackboardValueAccessor GetKeySetValueAccessor(IBlackboardKeyData keyData,
            IReadWriteBlackboardDataSet dataSet,
            IInstanceSynchronizer instanceSynchronizer) =>
            new KeySetValueAccessor(keyData, dataSet, instanceSynchronizer);

        public static IBlackboardKeyData GetKeyData(SerializableBlackboardKey[] keys) =>
            new KeyData(keys);

        public static IBlackboardValueAccessor GetMainValueAccessor(IBlackboardKeyData keyData) =>
            new MainValueAccessor(keyData);
    }
}