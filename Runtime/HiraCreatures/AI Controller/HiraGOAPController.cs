using System;
using System.Collections.Generic;
using System.Linq;
using Hiralal.GOAP.Actions;
using Hiralal.GOAP.Goals;

namespace UnityEngine
{
    [RequireComponent(typeof(HiraBlackboard))]
    public class HiraGOAPController : HiraController
    {
        [Space] [Header("Required Components")] [SerializeField]
        private HiraBlackboard blackboard = null;

        [SerializeField] private HiraPlanner planner = null;

        [Space] [Header("Behaviour Customization")] [SerializeField]
        private GoalSet goals = null;

        private Stack<HiraCreatureAction> _plan = new Stack<HiraCreatureAction>();
        private HiraCreatureAction _currentAction = null;

        public override void Possess(HiraCreature creature)
        {
            base.Possess(creature);

            planner.RequestPlan();

            blackboard.OnBlackboardUpdate += RecalculateGoal;
        }

        public override void Dispossess()
        {
            blackboard.OnBlackboardUpdate -= RecalculateGoal;

            base.Dispossess();
        }

        #region Goal Calculation

        private void RecalculateGoal()
        {
            var goal = goals.FirstOrDefault(g => g.ArePreConditionsSatisfied(blackboard.ValueSet));
            if (goal == planner.CurrentGoal) return;

            planner.CurrentGoal = goal;
        }

        #endregion

        protected void Iterate()
        {
            if (_plan == null)
            {
                if (planner.Plan.Value != null)
                {
                    _plan = planner.Plan.Value;
                    planner.Plan.Value = null;
                }
                else return;
            }

            if (_currentAction == null)
            {
                if (_plan.Count > 0) SetNextActionAsActive();
                else return;
            }

            _currentAction.OnActionExecute();
            
            var status = _currentAction.Status;
            switch (status)
            {
                case HiraActionStatus.Running:
                    break;
                case HiraActionStatus.Failed:
                    RequestPlan();
                    break;
                case HiraActionStatus.Succeeded:
                    if (_plan.Count == 0) RequestPlan();
                    else SetNextActionAsActive();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetNextActionAsActive()
        {
            _currentAction = _plan.Pop();
            _currentAction.OnActionStart();
        }

        private void RequestPlan()
        {
            _plan = null;
            _currentAction = null;
            planner.RequestPlan(true);
        }
    }
}