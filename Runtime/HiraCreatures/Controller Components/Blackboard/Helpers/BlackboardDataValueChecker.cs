using System;
using UnityEngine;

// ReSharper disable UnusedMember.Global
namespace HiraCreatures.Components.Blackboard.Helpers
{
    public static class BlackboardDataValueChecker
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        public static bool ContainsValue(this IBlackboardDataSet dataSet, IBlackboardValue value)
        {
            switch (value)
            {
                case IBlackboardValue<bool> booleanValue:
                    return dataSet.Booleans[booleanValue.TypeSpecificIndex] == booleanValue.TargetValue;
                case IBlackboardValue<float> floatValue:
                    return dataSet.Floats[floatValue.TypeSpecificIndex] == floatValue.TargetValue;
                case IBlackboardValue<int> intValue:
                    return dataSet.Integers[intValue.TypeSpecificIndex] == intValue.TargetValue;
                case IBlackboardValue<string> stringValue:
                    return dataSet.Strings[stringValue.TypeSpecificIndex] == stringValue.TargetValue;
                case IBlackboardValue<Vector3> vectorValue:
                    return dataSet.Vectors[vectorValue.TypeSpecificIndex] == vectorValue.TargetValue;
                default:
                    throw new InvalidCastException($"Type {value.GetType().GetGenericArguments()[0].Name} is not supported.");
            }
        }

        public static bool IsContainedBy(this IBlackboardValue value, IBlackboardDataSet dataSet) => dataSet.ContainsValue(value);
    }
}