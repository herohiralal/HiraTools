using System;
using System.Collections.Generic;
using System.Threading;
using HiraGOAP.Goals;
using Hiralal.GOAP.Planner;
using Hiralal.GOAP.Transitions;

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
        
        private bool _plannerActive = false;
        private HiraWorldStateTransition _currentGoal = null;
        private Stack<HiraCreatureAction> _queuedPlan = null;
        private Planner<HiraCreatureAction> _planner = null;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private void Awake()
        {
            GetNewPlanner();
        }

        private void OnDestroy()
        {
            _cts.Cancel();
        }

        private void GetNewPlanner()
        {
            _cts.Cancel();
            _plannerActive = false;
            _planner = new Planner<HiraCreatureAction>(blackboard, SetPlan);
            _cts = new CancellationTokenSource();
        }

        private bool RequestPlan()
        {
            if (_plannerActive) return false;

            _plannerActive = true;

            _planner.Initialize(maxFScore, _currentGoal, actions);
            
            if (multiThreaded) ThreadPool.QueueUserWorkItem(_planner.GeneratePlan, _cts.Token);
            else _planner.GeneratePlan(_cts.Token);

            return true;
        }

        private void SetPlan(PlannerResult result, Stack<HiraCreatureAction> newPlan)
        {
            _plannerActive = false;
            // if (newPlan != null) _queuedPlan = newPlan;
            switch (result)
            {
                case PlannerResult.None:
                    HiraLogger.LogErrorFormat(this, $"Planner returned an unhandled output.");
                    break;
                case PlannerResult.Success:
                    _queuedPlan = newPlan;
                    break;
                case PlannerResult.FScoreOverflow:
                    HiraLogger.LogWarning($"FScore overflow.", this);
                    break;
                case PlannerResult.Cancelled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        private void RecalculateGoal()
        {
            foreach (var goal in goals)
            {
                if (!goal.ArePreConditionsSatisfied(blackboard.ValueSet)) continue;
                if (_currentGoal == goal) return;

                _currentGoal = goal;

                if (RequestPlan()) return;
                
                GetNewPlanner();
                RequestPlan();
                return;
            }
        }
    }
}