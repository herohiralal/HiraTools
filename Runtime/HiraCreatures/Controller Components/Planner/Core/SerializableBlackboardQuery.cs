using System;
using HiraEngine.Components.Planner;
using HiraEngine.Components.Planner.Internal;

namespace UnityEngine
{
    [Serializable]
    public class SerializableBlackboardQuery : SerializableBlackboardValue
    {
        public IBlackboardQuery Query => PlannerTypes.GetQuery(typeString, this);
    }

    [Serializable]
    public class SerializableBlackboardModification : SerializableBlackboardValue
    {
        public IBlackboardModification Modification => PlannerTypes.GetModification(typeString, this);
    }

    [Serializable]
    public class SerializableBlackboardHybridValue : SerializableBlackboardValue
    {
        public IBlackboardHybridValue HybridValue => PlannerTypes.GetHybridValue(typeString, this);
    }
}