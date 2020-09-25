using System;
using HiraEngine.Components.Planner.Internal;

namespace UnityEngine
{
    [Serializable]
    public class SerializableBlackboardQuery : SerializableBlackboardValue
    {
        public IBlackboardQuery Query => PlannerTypes.GetQuery(typeString, this);
    }
}