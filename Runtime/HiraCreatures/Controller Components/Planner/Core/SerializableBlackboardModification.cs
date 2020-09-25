using System;
using HiraEngine.Components.Planner.Internal;

namespace UnityEngine
{
    [Serializable]
    public class SerializableBlackboardModification : SerializableBlackboardValue
    {
        public IBlackboardModification Modification => PlannerTypes.GetModification(typeString, this);
    }
}