using System;
using UnityEngine;

namespace Hiralal.Blackboard
{
    public class HiraBlackboardValueSet
    {
        public HiraBlackboardValueSet(uint boolKeyCount, uint floatKeyCount, uint intKeyCount, uint stringKeyCount, uint vectorKeyCount)
        {
            booleans = new bool[boolKeyCount];
            floats = new float[floatKeyCount];
            integers = new int[intKeyCount];
            strings = new string[stringKeyCount];
            vectors = new Vector3[vectorKeyCount];
        }
        
        public readonly bool[] booleans = null;
        public readonly float[] floats = null;
        public readonly int[] integers = null;
        public readonly string[] strings = null;
        public readonly Vector3[] vectors = null;

        public HiraBlackboardValueSet Copy()
        {
            var copy = new HiraBlackboardValueSet((uint) booleans.Length,
                (uint) floats.Length,
                (uint) integers.Length,
                (uint) strings.Length,
                (uint) vectors.Length);

            for (var i = 0; i < booleans.Length; i++) copy.booleans[i] = booleans[i];
            for (var i = 0; i < floats.Length; i++) copy.floats[i] = floats[i];
            for (var i = 0; i < integers.Length; i++) copy.integers[i] = integers[i];
            for (var i = 0; i < strings.Length; i++) copy.strings[i] = strings[i];
            for (var i = 0; i < vectors.Length; i++) copy.vectors[i] = vectors[i];

            return copy;
        }

        public bool ContainsValue(HiraBlackboardValue value)
        {
            switch (value)
            {
                case HiraBlackboardValue<bool> booleanValue:
                    return booleans[booleanValue.TypeSpecificIndex] == booleanValue.TargetValue;
                case HiraBlackboardValue<float> floatValue:
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    return floats[floatValue.TypeSpecificIndex] == floatValue.TargetValue;
                case HiraBlackboardValue<int> intValue:
                    return integers[intValue.TypeSpecificIndex] == intValue.TargetValue;
                case HiraBlackboardValue<string> stringValue:
                    return strings[stringValue.TypeSpecificIndex] == stringValue.TargetValue;
                case HiraBlackboardValue<Vector3> vectorValue:
                    return vectors[vectorValue.TypeSpecificIndex] == vectorValue.TargetValue;
                default:
                    throw new InvalidCastException($"Type {value.GetType().GetGenericArguments()[0].Name} is not supported.");
            }
        }
    }
}