using UnityEngine;

namespace HiraEngine.Components.Blackboard
{
    public interface IBlackboardValue
    {
        bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet);
    }

    public interface IBlackboardValueDefaultObject<in T> : IBlackboardValue
    {
        IBlackboardValue GetNewObject(uint typeSpecificIndex, T value);
    }
}