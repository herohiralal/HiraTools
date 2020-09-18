using HiraCreatures.Components.Blackboard.Internal;
using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Helpers
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

        public static IBlackboardKeyData GetKeyData(SerializableKey[] keys) =>
            new KeyData(keys);

        public static IBlackboardValueAccessor GetMainValueAccessor(IBlackboardKeyData keyData) =>
            new MainValueAccessor(keyData);

        public static IBlackboardValue GetValue(string typeString,
            IBlackboardValueConstructorParams constructorParams) =>
            BlackboardValueFactory.GetValue(typeString, constructorParams);
    }
}