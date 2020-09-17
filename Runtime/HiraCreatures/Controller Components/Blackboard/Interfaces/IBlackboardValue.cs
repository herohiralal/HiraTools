using UnityEngine;

namespace HiraCreatures.Components.Blackboard
{
    public interface IBlackboardValue
    {
        bool IsSatisfiedBy(IReadOnlyBlackboardDataSet dataSet);
    }
}