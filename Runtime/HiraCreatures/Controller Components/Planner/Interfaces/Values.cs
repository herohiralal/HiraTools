using UnityEngine;

namespace HiraEngine.Components.Planner
{
    public interface IBlackboardModificationDefaultObject<in T> : IBlackboardModification
    {
        IBlackboardModification GetNewModificationObject(uint typeSpecificIndex, T value);
    }

    public interface IBlackboardQueryDefaultObject<in T> : IBlackboardQuery
    {
        IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, T value);
    }

    public interface IBlackboardHybridValue : IBlackboardQuery, IBlackboardModification
    {
    }

    public interface IBlackboardHybridValueDefaultObject<in T> : IBlackboardHybridValue,
        IBlackboardQueryDefaultObject<T>,
        IBlackboardModificationDefaultObject<T>
    {
        IBlackboardHybridValue GetNewHybridObject(uint typeSpecificIndex, T value);
    }
}