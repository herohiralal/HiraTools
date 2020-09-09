using System.Collections.Generic;
using Hiralal.Blackboard;
using UnityEngine;

namespace Hiralal.GOAP.Planner
{
    internal static class PlannerHelper
    {
        internal static IReadOnlyList<HiraBlackboardValue> ApplyAction(this HiraBlackboardValueSet state, IHiraAction action)
        {
            var effects = action.TransitionComponent.Effects;
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
                            state.booleans[booleanTypeSpecificIndex]);
                        state.booleans[booleanTypeSpecificIndex] = booleanValue.TargetValue;
                        break;
                    case HiraBlackboardValue<float> floatValue:
                        var floatTypeSpecificIndex = floatValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<float>(floatTypeSpecificIndex,
                            state.floats[floatTypeSpecificIndex]);
                        state.floats[floatTypeSpecificIndex] = floatValue.TargetValue;
                        break;
                    case HiraBlackboardValue<int> intValue:
                        var intTypeSpecificIndex = intValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<int>(intTypeSpecificIndex,
                            state.integers[intTypeSpecificIndex]);
                        state.integers[intTypeSpecificIndex] = intValue.TargetValue;
                        break;
                    case HiraBlackboardValue<string> stringValue:
                        var stringTypeSpecificIndex = stringValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<string>(stringTypeSpecificIndex,
                            state.strings[stringTypeSpecificIndex]);
                        state.strings[stringTypeSpecificIndex] = stringValue.TargetValue;
                        break;
                    case HiraBlackboardValue<Vector3> vectorValue:
                        var vectorTypeSpecificIndex = vectorValue.TypeSpecificIndex;
                        undoBuffer[i] = new HiraBlackboardValue<Vector3>(vectorTypeSpecificIndex,
                            state.vectors[vectorTypeSpecificIndex]);
                        state.vectors[vectorTypeSpecificIndex] = vectorValue.TargetValue;
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
                        state.booleans[booleanValue.TypeSpecificIndex] = booleanValue.TargetValue;
                        break;
                    case HiraBlackboardValue<float> floatValue:
                        state.floats[floatValue.TypeSpecificIndex] = floatValue.TargetValue;
                        break;
                    case HiraBlackboardValue<int> intValue:
                        state.integers[intValue.TypeSpecificIndex] = intValue.TargetValue;
                        break;
                    case HiraBlackboardValue<string> stringValue:
                        state.strings[stringValue.TypeSpecificIndex] = stringValue.TargetValue;
                        break;
                    case HiraBlackboardValue<Vector3> vectorValue:
                        state.vectors[vectorValue.TypeSpecificIndex] = vectorValue.TargetValue;
                        break;
                }
            }
        }

        internal static bool DoesNotContainValue(this HiraBlackboardValueSet valueSet, HiraBlackboardValue value) =>
            !valueSet.ContainsValue(value);
    }
}