using System.Collections.Generic;
using Hiralal.Blackboard;
using Hiralal.GOAP.Actions;
using Hiralal.GOAP.Transitions;

namespace UnityEngine
{
    public abstract class HiraCreatureAction : IHiraAction
    {
        protected HiraCreatureAction(HiraWorldStateTransition transition) => Transition = transition;
        
        protected readonly HiraWorldStateTransition Transition = null;
        public bool ArePreConditionsSatisfied(HiraBlackboardValueSet valueSet) => Transition.ArePreConditionsSatisfied(valueSet);
        public IReadOnlyList<HiraBlackboardValue> Effects => Transition.Effects;
        public abstract float Cost { get; }
    }
}