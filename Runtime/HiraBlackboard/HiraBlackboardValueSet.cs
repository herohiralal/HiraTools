using System;
using UnityEngine;

namespace Hiralal.Blackboard
{
    public class HiraBlackboardValueSet
    {
        public HiraBlackboardValueSet(uint boolKeyCount, uint floatKeyCount, uint intKeyCount, uint stringKeyCount, uint vectorKeyCount)
        {
            Booleans = new bool[boolKeyCount];
            Floats = new float[floatKeyCount];
            Integers = new int[intKeyCount];
            Strings = new string[stringKeyCount];
            Vectors = new Vector3[vectorKeyCount];
        }
        
        public readonly bool[] Booleans = null;
        public readonly float[] Floats = null;
        public readonly int[] Integers = null;
        public readonly string[] Strings = null;
        public readonly Vector3[] Vectors = null;

        public HiraBlackboardValueSet Copy()
        {
            var copy = new HiraBlackboardValueSet((uint) Booleans.Length,
                (uint) Floats.Length,
                (uint) Integers.Length,
                (uint) Strings.Length,
                (uint) Vectors.Length);

            for (var i = 0; i < Booleans.Length; i++) copy.Booleans[i] = Booleans[i];
            for (var i = 0; i < Floats.Length; i++) copy.Floats[i] = Floats[i];
            for (var i = 0; i < Integers.Length; i++) copy.Integers[i] = Integers[i];
            for (var i = 0; i < Strings.Length; i++) copy.Strings[i] = Strings[i];
            for (var i = 0; i < Vectors.Length; i++) copy.Vectors[i] = Vectors[i];

            return copy;
        }

        public bool ContainsValue(HiraBlackboardValue value)
        {
            switch (value)
            {
                case HiraBlackboardValue<bool> booleanValue:
                    return Booleans[booleanValue.TypeSpecificIndex] == booleanValue.TargetValue;
                case HiraBlackboardValue<float> floatValue:
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    return Floats[floatValue.TypeSpecificIndex] == floatValue.TargetValue;
                case HiraBlackboardValue<int> intValue:
                    return Integers[intValue.TypeSpecificIndex] == intValue.TargetValue;
                case HiraBlackboardValue<string> stringValue:
                    return Strings[stringValue.TypeSpecificIndex] == stringValue.TargetValue;
                case HiraBlackboardValue<Vector3> vectorValue:
                    return Vectors[vectorValue.TypeSpecificIndex] == vectorValue.TargetValue;
                default:
                    throw new InvalidCastException($"Type {value.GetType().GetGenericArguments()[0].Name} is not supported.");
            }
        }
    }
}