using System;
using Hiralal.Blackboard;
using UnityEngine;

namespace Hiralal.GOAP
{
    [Serializable]
    public class HiraSerializableBlackboardValue
    {
        [SerializeField] private StringReference name = null;
        [SerializeField] private HiraBlackboardKeyType keyType = HiraBlackboardKeyType.Undefined;

        [SerializeField] private bool boolValue = false;
        [SerializeField] private float floatValue = 0f;
        [SerializeField] private int intValue = 0;
        [SerializeField] private string stringValue = "";
        [SerializeField] private Vector3 vectorValue = Vector3.zero;

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
                    return new HiraBlackboardValue<bool>(typeSpecificIndex, boolValue);
                case HiraBlackboardKeyType.Float:
                    return new HiraBlackboardValue<float>(typeSpecificIndex, floatValue);
                case HiraBlackboardKeyType.Int:
                    return new HiraBlackboardValue<int>(typeSpecificIndex, intValue);
                case HiraBlackboardKeyType.String:
                    return new HiraBlackboardValue<string>(typeSpecificIndex, stringValue);
                case HiraBlackboardKeyType.Vector:
                    return new HiraBlackboardValue<Vector3>(typeSpecificIndex, vectorValue);
                default:
                    Debug.LogErrorFormat($"Key {name}'s type is invalid.");
                    return null;
            }
        }
    }
}