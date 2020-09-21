using UnityEngine;

namespace HiraEngine.Components.Planner
{
    public interface IBlackboardModificationDefaultObject<in T> : IBlackboardModification
    {
        IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, T value);
    }
}