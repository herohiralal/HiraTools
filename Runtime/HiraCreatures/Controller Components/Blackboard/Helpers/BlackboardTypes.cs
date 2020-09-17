using HiraCreatures.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Helpers
{
    public static class BlackboardTypes
    {
        public static IBlackboardDataSet GetDataSet(uint boolKeyCount, uint floatKeyCount, uint intKeyCount,
            uint stringKeyCount, uint vectorKeyCount) =>
            new DataSet(boolKeyCount, floatKeyCount, intKeyCount, stringKeyCount, vectorKeyCount);

        public static IInstanceSynchronizer GetSynchronizer() =>
            new InstanceSynchronizer();

        public static IBlackboardValueAccessor GetKeySetValueAccessor(IBlackboardKeyData keyData,
            IBlackboardDataSet dataSet,
            IInstanceSynchronizer instanceSynchronizer) =>
            new KeySetValueAccessor(keyData, dataSet, instanceSynchronizer);

        public static IBlackboardKeyData GetKeyData(SerializableKey[] keys) =>
            new KeyData(keys);

        public static IBlackboardValueAccessor GetMainValueAccessor(IBlackboardKeyData keyData) =>
            new MainValueAccessor(keyData);
    }
}