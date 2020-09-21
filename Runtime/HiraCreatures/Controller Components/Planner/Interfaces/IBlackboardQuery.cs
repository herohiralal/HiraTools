using UnityEngine;

namespace HiraEngine.Components.Planner
{
    public interface IBlackboardQueryDefaultObject<in T> : IBlackboardQuery
    {
        IBlackboardQuery GetNewQueryObject(uint typeSpecificIndex, T value);
    }
}