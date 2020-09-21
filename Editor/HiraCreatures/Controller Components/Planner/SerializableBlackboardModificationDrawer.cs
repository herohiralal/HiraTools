using HiraEditor.HiraEngine.Components.Planner.Helpers;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.HiraEngine.Components.Planner
{
    [CustomPropertyDrawer(typeof(SerializableBlackboardModification))]
    public class SerializableBlackboardModificationDrawer : SerializableBlackboardValueDrawerBase
    {
        protected override (string[], string[]) BuildCalculationData(SerializableBlackboardKey key) => 
            key.BuildModificationCalculationData();
    }
}