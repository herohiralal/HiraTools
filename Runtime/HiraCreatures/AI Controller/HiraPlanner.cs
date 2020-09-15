using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Hiralal.GOAP.Planner;
using Hiralal.GOAP.Transitions;
using Hiralal.Utilities;

namespace UnityEngine
{
    public class HiraPlanner : MonoBehaviour
    {
        [Space, Header("Required Components"), SerializeField]
        private HiraBlackboard blackboard = null;

        [Space, Header("Settings"), SerializeField]
        private BoolReference multiThreaded = null;
        [SerializeField] private FloatReference maxFScore = null;

        [Space, Header("Data"), SerializeField]
        private HiraWorldStateTransition currentGoal = null;

        private bool _planRequested;
        private Planner<HiraCreatureAction> _planner = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public ThreadSafeObject<Stack<HiraCreatureAction>> Plan { get; } =
            new ThreadSafeObject<Stack<HiraCreatureAction>>();

        public HiraWorldStateTransition CurrentGoal
        {
            get => currentGoal;
            set => currentGoal = value;
        }

        private readonly List<HiraCreatureAction> _actions = new List<HiraCreatureAction>();

        public void AddAction(HiraCreatureAction action)
        {
            if (!_actions.Contains(action)) _actions.Add(action);
        }

        public void RemoveAction(HiraCreatureAction action)
        {
            if (_actions.Contains(action)) _actions.Remove(action);
        }

        private void Awake()
        {
            _planner = new Planner<HiraCreatureAction>(blackboard, PlannerCallback);
        }

        public void RequestPlan(bool forceRestartPlanner = false)
        {
            if (forceRestartPlanner && _planner.IsActive) _cts.Cancel();
            if (_planRequested) return;
            StartCoroutine(RequestPlanEnumerator());
        }

        private IEnumerator RequestPlanEnumerator()
        {
            _planRequested = true;
            while (_planner.IsActive) yield return null;

            if (_cts.IsCancellationRequested) _cts = new CancellationTokenSource();
            
            _planner.Initialize(maxFScore, currentGoal, _actions);
            if (multiThreaded) ThreadPool.QueueUserWorkItem(_planner.GeneratePlan, _cts.Token);
            else _planner.GeneratePlan(CancellationToken.None);
            _planRequested = false;
        }

        private void PlannerCallback(PlannerResult result, Stack<HiraCreatureAction> newPlan)
        {
            switch (result)
            {
                case PlannerResult.None:
                    HiraLogger.LogErrorFormat(this, $"Planner returned an unhandled output.");
                    break;
                case PlannerResult.Success:
                    Plan.Value = newPlan;
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
    }
}