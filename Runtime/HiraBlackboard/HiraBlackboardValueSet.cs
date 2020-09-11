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

            Copy(this, copy);

            return copy;
        }

        public static void Copy(HiraBlackboardValueSet source, HiraBlackboardValueSet destination)
        {
            for (var i = 0; i < source.Booleans.Length; i++) destination.Booleans[i] = source.Booleans[i];
            for (var i = 0; i < source.Floats.Length; i++) destination.Floats[i] = source.Floats[i];
            for (var i = 0; i < source.Integers.Length; i++) destination.Integers[i] = source.Integers[i];
            for (var i = 0; i < source.Strings.Length; i++) destination.Strings[i] = source.Strings[i];
            for (var i = 0; i < source.Vectors.Length; i++) destination.Vectors[i] = source.Vectors[i];
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