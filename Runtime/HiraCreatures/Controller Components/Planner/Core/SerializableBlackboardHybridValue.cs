using System;
using HiraEngine.Components.Planner;
using HiraEngine.Components.Planner.Internal;

namespace UnityEngine
{
    [Serializable]
    public class SerializableBlackboardHybridValue : SerializableBlackboardValue
    {
        public IBlackboardHybridValue HybridValue => PlannerTypes.GetHybridValue(typeString, this);
    }
}