using HiraEditor.HiraEngine.Components.Planner.Helpers;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.HiraEngine.Components.Planner
{
    [CustomPropertyDrawer(typeof(SerializableBlackboardHybridValue))]
    public class SerializableBlackboardHybridValueDrawer : SerializableBlackboardValueDrawerBase
    {
        protected override (string[], string[]) BuildCalculationData(SerializableBlackboardKey key) => 
            key.BuildHybridCalculationData();
    }
}