using System;
using Hiralal.Blackboard;
using UnityEngine;

namespace Hiralal.GOAP.Transitions
{
    [Serializable]
    public class HiraSerializableBlackboardValue
    {
        [SerializeField] private StringReference name = null;
        [SerializeField] private HiraBlackboardKeyType keyType = HiraBlackboardKeyType.Undefined;

        [SerializeField] private BoolReference boolValue = null;
        [SerializeField] private FloatReference floatValue = null;
        [SerializeField] private IntReference intValue = null;
        [SerializeField] private StringReference stringValue = null;
        [SerializeField] private Vector3Reference vectorValue = null;

        internal HiraBlackboardValue GetBlackboardValue(HiraBlackboardKeySet keySet)
        {
            uint hash;
            try
            {
                hash = keySet.GetHash(name);
            }
            catch (NullReferenceException)
            {
                Debug.LogErrorFormat($"{name} is not a valid key.");
                return null;
            }

            var typeSpecificIndex = keySet.GetTypeSpecificIndex(hash);

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (keyType)
            {
                case HiraBlackboardKeyType.Bool:
                    return new HiraBlackboardValue<bool>(typeSpecificIndex, boolValue.Value);
                case HiraBlackboardKeyType.Float:
                    return new HiraBlackboardValue<float>(typeSpecificIndex, floatValue.Value);
                case HiraBlackboardKeyType.Int:
                    return new HiraBlackboardValue<int>(typeSpecificIndex, intValue.Value);
                case HiraBlackboardKeyType.String:
                    return new HiraBlackboardValue<string>(typeSpecificIndex, stringValue.Value);
                case HiraBlackboardKeyType.Vector:
                    return new HiraBlackboardValue<Vector3>(typeSpecificIndex, vectorValue.Value);
                default:
                    Debug.LogErrorFormat($"Key {name}'s type is invalid.");
                    return null;
            }
        }
    }
}