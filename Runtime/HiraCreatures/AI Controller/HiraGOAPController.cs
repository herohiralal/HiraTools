using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Hiralal.GOAP.Goals;
using Hiralal.GOAP.Planner;
using Hiralal.GOAP.Transitions;
using Hiralal.Utilities;

namespace UnityEngine
{
    [RequireComponent(typeof(HiraBlackboard))]
    public class HiraGOAPController : HiraController
    {
        [Space] [Header("Required Components")]
        [SerializeField] private HiraBlackboard blackboard = null;
        
        [Space] [Header("Behaviour Customization")]
        [SerializeField] private GoalSet goals = null;
        private List<HiraCreatureAction> actions = null;

        [Space] [Header("Planner Settings")] 
        [SerializeField] private BoolReference multiThreaded = null;
        [SerializeField] private FloatReference maxFScore = null;
        
        private readonly ThreadSafeObject<bool> _plannerActive = new ThreadSafeObject<bool>(false);
        private readonly ThreadSafeObject<bool> _planRequested = new ThreadSafeObject<bool>(false);
        private readonly ThreadSafeObject<Stack<HiraCreatureAction>> _queuedPlan = new ThreadSafeObject<Stack<HiraCreatureAction>>();
        
        
        private HiraWorldStateTransition _currentGoal = null;
        private Planner<HiraCreatureAction> _planner = null;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public override void Possess(HiraCreature creature)
        {
            base.Possess(creature);

            blackboard.OnBlackboardUpdate += RecalculateGoal;
            _planner = new Planner<HiraCreatureAction>(blackboard, PlannerCallback);
        }

        public override void Dispossess()
        {
            _cts.Cancel();
            _planner = null;
            blackboard.OnBlackboardUpdate -= RecalculateGoal;
            
            base.Dispossess();
        }

        #region Planning

        private void RequestPlan()
        {
            if (_planRequested.Value) return;
            _planRequested.Value = true;
            StartCoroutine(RequestPlanEnumerator());
        }

        private IEnumerator RequestPlanEnumerator()
        {
            while (_plannerActive.Value) yield return null;
            _plannerActive.Value = true;
            
            if(_cts.IsCancellationRequested) _cts = new CancellationTokenSource();
            
            _planner.Initialize(maxFScore, _currentGoal, actions);
            if (multiThreaded) ThreadPool.QueueUserWorkItem(_planner.GeneratePlan, _cts.Token);
            else _planner.GeneratePlan(CancellationToken.None);
        }
        
        private void PlannerCallback(PlannerResult result, Stack<HiraCreatureAction> newPlan)
        {
            switch (result)
            {
                case PlannerResult.None:
                    HiraLogger.LogErrorFormat(this, $"Planner returned an unhandled output.");
                    break;
                case PlannerResult.Success:
                    _queuedPlan.Value = newPlan;
                    break;
                case PlannerResult.FScoreOverflow:
                    HiraLogger.LogWarning($"FScore overflow.", this);
                    break;
                case PlannerResult.Cancelled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
            _planRequested.Value = false;
            _plannerActive.Value = false;
        }
        
        #endregion
        
        #region Goal Calculation

        private void RecalculateGoal()
        {
            var goal = goals.FirstOrDefault(g => g.ArePreConditionsSatisfied(blackboard.ValueSet));
            if (goal == _currentGoal) return;

            _currentGoal = goal;
            RequestPlan();
        }
        
        #endregion
    }
}