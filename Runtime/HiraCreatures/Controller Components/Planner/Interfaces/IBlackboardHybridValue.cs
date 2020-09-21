using UnityEngine;

namespace HiraEngine.Components.Planner
{
    public interface IBlackboardHybridValue : IBlackboardQuery, IBlackboardModification
    {
        
    }

    public interface IBlackboardHybridValueDefaultObject<in T> : IBlackboardHybridValue, IBlackboardQueryDefaultObject<T>,
        IBlackboardModificationDefaultObject<T>
    {
        IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, T value);
    }
}