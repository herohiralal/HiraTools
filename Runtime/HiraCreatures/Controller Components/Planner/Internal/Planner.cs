using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class Planner<T> : IPlanner<T> where T : IAction
    {
        public Planner(IBlackboardValueAccessor valueAccessor, byte planLength)
        {
            _valueAccessor = valueAccessor;
            _dataSets = new IReadWriteBlackboardDataSet[planLength + 1];
            _dataSets[0] = valueAccessor.DataSet.GetDuplicate();
            for (var i = 1; i < planLength + 1; i++) _dataSets[i] = _dataSets[0].GetDuplicate();
        }

        private readonly ThreadSafeObject<bool> _isActive = new ThreadSafeObject<bool>(false);
        public bool IsActive => _isActive.Value;

        private readonly IBlackboardValueAccessor _valueAccessor;
        private readonly IReadWriteBlackboardDataSet[] _dataSets;

        private IEnumerable<IBlackboardQuery> _goal = null;
        private IEnumerable<T> _actions = null;

        private float _maxFScore = 0f;
        private CancellationToken _ct = CancellationToken.None;
        private event PlannerCompletionCallback<T> OnPlannerFinish = delegate { };
        private T[] _plan = null;

        public IPlanner<T> Initialize()
        {
            _valueAccessor.DataSet.CopyTo(_dataSets[0]);
            return this;
        }

        public IPlanner<T> ForGoal(IEnumerable<IBlackboardQuery> goal)
        {
            _goal = goal;
            return this;
        }

        public IPlanner<T> WithAvailableTransitions(IEnumerable<T> actions)
        {
            _actions = actions;
            foreach (var action in _actions) action.BuildPrePlanCache();
            return this;
        }

        public IPlanner<T> WithMaxFScore(float maxFScore)
        {
            _maxFScore = maxFScore;
            return this;
        }

        public IPlanner<T> WithCancellationToken(CancellationToken ct)
        {
            _ct = ct;
            return this;
        }

        public void WithCallback(PlannerCompletionCallback<T> completionCallback)
        {
            OnPlannerFinish = completionCallback;
        }

        public void GeneratePlan(object obj = null)
        {
            PlannerResult result;

            float threshold = GetHeuristic(0);

            while (true)
            {
                var score = _ct.IsCancellationRequested
                    ? -1 
                    : PerformHeuristicEstimatedSearch(1, 0, threshold);

                if (!score.HasValue)
                {
                    result = PlannerResult.Success;
                    break;
                }

                if (score.Value > _maxFScore)
                {
                    result = PlannerResult.FScoreOverflow;
                    break;
                }

                if (score.Value < 0)
                {
                    result = PlannerResult.Cancelled;
                    break;
                }

                threshold = score.Value;
            }

            OnPlannerFinish(result, _plan);

            _plan = null;
            _goal = null;
            _actions = null;
            _maxFScore = 0f;
            OnPlannerFinish = delegate { };
            _isActive.Value = false;
            _ct = CancellationToken.None;
        }

        private int GetHeuristic(int index) => _goal.Count(_dataSets[index].DoesNotSatisfy);

        private float? PerformHeuristicEstimatedSearch(int index, float cost, float threshold)
        {
            var heuristic = GetHeuristic(index - 1);
            var fScore = cost + heuristic;
            if (fScore > threshold) return fScore;

            if (heuristic == 0)
            {
                _plan = new T[index - 1];
                return null;
            }

            if (index == _dataSets.Length) return float.MaxValue;

            var min = float.MaxValue;
            foreach (var action in _actions)
            {
                if (action.Preconditions.IsNotSatisfiedBy(_dataSets[index - 1])) continue;

                _dataSets[index - 1].CopyTo(_dataSets[index]);
                action.Effects.ApplyTo(_dataSets[index]);

                var score = _ct.IsCancellationRequested
                    ? -1
                    : PerformHeuristicEstimatedSearch(index + 1, cost + action.Cost, threshold);

                _dataSets[index - 1].CopyTo(_dataSets[index]);

                if (!score.HasValue)
                {
                    _plan[index - 1] = action;
                    return null;
                }

                if (score.Value < min) min = score.Value;
            }

            return min;
        }
    }
}