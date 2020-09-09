using System.Collections.Generic;
using Hiralal.Blackboard;

namespace Hiralal.GOAP.Transitions
{
    public interface IHiraWorldStateTransition
    {
        bool ArePreConditionsSatisfied(HiraBlackboardValueSet valueSet);
        IReadOnlyList<HiraBlackboardValue> Effects { get; }
        float Cost { get; }
    }
}