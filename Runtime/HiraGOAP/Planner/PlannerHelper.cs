using System.Collections.Generic;
using Hiralal.Blackboard;
using Hiralal.GOAP.Transitions;
using UnityEngine;

namespace Hiralal.GOAP.Planner
{
    internal static class PlannerHelper
    {
        internal static IReadOnlyList<HiraBlackboardValue> ApplyAction(this HiraBlackboardValueSet state, IHiraWorldStateTransition transition)
        {
            var effects = transition.Effects;
            var count = effects.Count;
            var undoBuffer = new HiraBlackboardValue[count];

            for (var i = 0; i < count; i++)
            {
                var effect = effects[i];
                switch (effect)
                {
                    case HiraBlackboardValue<bool> booleanValue:
                        var booleanTypeSpecificIndex = booleanValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<bool>(booleanTypeSpecificIndex,
                            state.Booleans[booleanTypeSpecificIndex]);
                        state.Booleans[booleanTypeSpecificIndex] = booleanValue.TargetValue;
                        break;
                    case HiraBlackboardValue<float> floatValue:
                        var floatTypeSpecificIndex = floatValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<float>(floatTypeSpecificIndex,
                            state.Floats[floatTypeSpecificIndex]);
                        state.Floats[floatTypeSpecificIndex] = floatValue.TargetValue;
                        break;
                    case HiraBlackboardValue<int> intValue:
                        var intTypeSpecificIndex = intValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<int>(intTypeSpecificIndex,
                            state.Integers[intTypeSpecificIndex]);
                        state.Integers[intTypeSpecificIndex] = intValue.TargetValue;
                        break;
                    case HiraBlackboardValue<string> stringValue:
                        var stringTypeSpecificIndex = stringValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<string>(stringTypeSpecificIndex,
                            state.Strings[stringTypeSpecificIndex]);
                        state.Strings[stringTypeSpecificIndex] = stringValue.TargetValue;
                        break;
                    case HiraBlackboardValue<Vector3> vectorValue:
                        var vectorTypeSpecificIndex = vectorValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<Vector3>(vectorTypeSpecificIndex,
                            state.Vectors[vectorTypeSpecificIndex]);
                        state.Vectors[vectorTypeSpecificIndex] = vectorValue.TargetValue;
                        break;
                }
            }

            return undoBuffer;
        }

        internal static void Undo(this HiraBlackboardValueSet state, IReadOnlyList<HiraBlackboardValue> undoBuffer)
        {
            for (var count = undoBuffer.Count - 1; count > -1; count--)
            {
                switch (undoBuffer[count])
                {
                    case HiraBlackboardValue<bool> booleanValue:
                        state.Booleans[booleanValue.TypeSpecificIndex] = booleanValue.TargetValue;
                        break;
                    case HiraBlackboardValue<float> floatValue:
                        state.Floats[floatValue.TypeSpecificIndex] = floatValue.TargetValue;
                        break;
                    case HiraBlackboardValue<int> intValue:
                        state.Integers[intValue.TypeSpecificIndex] = intValue.TargetValue;
                        break;
                    case HiraBlackboardValue<string> stringValue:
                        state.Strings[stringValue.TypeSpecificIndex] = stringValue.TargetValue;
                        break;
                    case HiraBlackboardValue<Vector3> vectorValue:
                        state.Vectors[vectorValue.TypeSpecificIndex] = vectorValue.TargetValue;
                        break;
                }
            }
        }

        internal static bool DoesNotContainValue(this HiraBlackboardValueSet valueSet, HiraBlackboardValue value) =>
            !valueSet.ContainsValue(value);
    }
}