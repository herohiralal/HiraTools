using HiraEditor.HiraEngine.Components.Planner.Helpers;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.HiraEngine.Components.Planner
{
    [CustomPropertyDrawer(typeof(SerializableBlackboardQuery))]
    public class SerializableBlackboardQueryDrawer : SerializableBlackboardValueDrawerBase
    {
        protected override (string[], string[]) BuildCalculationData(SerializableBlackboardKey key) => 
            key.BuildQueryCalculationData();
    }
}