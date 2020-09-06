using System.Collections.Generic;
using Hiralal.Blackboard;

namespace Hiralal.GOAP
{
    public interface IHiraWorldStateTransition
    {
        bool ArePreConditionsSatisfied(HiraBlackboardValueSet valueSet);
        IReadOnlyList<HiraBlackboardValue> Effects { get; }
        float BaseCost { get; }
    }
}