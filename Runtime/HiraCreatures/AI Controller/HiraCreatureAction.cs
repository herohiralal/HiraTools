using System.Collections.Generic;
using System.Linq;
using Hiralal.Blackboard;
using Hiralal.GOAP.Actions;

namespace UnityEngine
{
    public abstract class HiraCreatureAction : IHiraAction
    {
        protected HiraCreatureAction()
        {
        }

        protected HiraCreatureAction(IEnumerable<HiraBlackboardValue> preconditions, IReadOnlyList<HiraBlackboardValue> effects)
        {
            _preconditions = preconditions;
            Effects = effects;
        }

        private readonly IEnumerable<HiraBlackboardValue> _preconditions;
        public abstract bool IsApplicableTo(HiraCreature creature);

        public virtual bool ArePreConditionsSatisfied(HiraBlackboardValueSet valueSet) =>
            _preconditions.All(valueSet.ContainsValue);
        public virtual IReadOnlyList<HiraBlackboardValue> Effects { get; }
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

        public abstract void OnActionExecute();
    }
}