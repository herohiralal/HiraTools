using System.Collections.Generic;
using Hiralal.Blackboard;
using Hiralal.GOAP.Actions;
using Hiralal.GOAP.Transitions;

namespace UnityEngine
{
    public abstract class HiraCreatureAction : IHiraAction
    {
        protected HiraCreatureAction()
        {
        }

        protected HiraCreatureAction(HiraWorldStateTransition transition) => Transition = transition;
        
        protected readonly HiraWorldStateTransition Transition = null;
        public abstract bool IsApplicableTo(HiraCreature creature);
        public virtual bool ArePreConditionsSatisfied(HiraBlackboardValueSet valueSet) => Transition.ArePreConditionsSatisfied(valueSet);
        public virtual IReadOnlyList<HiraBlackboardValue> Effects => Transition.Effects;
        public abstract float Cost { get; }
        public virtual void BuildPrePlanCache()
        {
            
        }

        public HiraActionStatus Status { get; private set; } = HiraActionStatus.None;
        protected void MarkCompleted() => Status = HiraActionStatus.Succeeded;
        protected void MarkFailed() => Status = HiraActionStatus.Failed;
        
        public abstract HiraCreature TargetCreature { set; }
        public virtual void OnActionStart()
        {
            Status = HiraActionStatus.Running;
        }

        public virtual void OnActionExecute()
        {
            
        }
    }
}