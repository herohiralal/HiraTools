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
            _dataSets = new IReadWriteBlackboardDataSet[planLength+1];
            _dataSets[0] = valueAccessor.DataSet.GetDuplicate();
            for (var i = 1; i < planLength+1; i++) _dataSets[i] = _dataSets[0].GetDuplicate();
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
            for (var i = 1; i < _dataSets.Length; i++) _dataSets[0].CopyTo(_dataSets[i]);
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

            float threshold = GetHeuristic(-1);

            while (true)
            {
                var score = PerformHeuristicEstimatedSearch(0, 0, threshold);

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

        private int GetHeuristic(int depth) => _goal.Count(_dataSets[depth+1].DoesNotSatisfy);

        private float? PerformHeuristicEstimatedSearch(int depth, float cost, float threshold)
        {
            if (_ct.IsCancellationRequested) return -1;
            if (depth > _dataSets.Length - 2) return cost + GetHeuristic(_dataSets.Length - 2);
            
            var heuristic = GetHeuristic(depth);
            var fScore = cost + heuristic;
            if (fScore > threshold) return fScore;

            if (heuristic == 0)
            {
                _plan = new T[depth];
                return null;
            }

            var min = float.MaxValue;
            var index = depth + 1;
            foreach (var action in _actions)
            {
                if (action.Preconditions.IsNotSatisfiedBy(_dataSets[index])) continue;

                action.Effects.ApplyTo(_dataSets[index]);

                var score = PerformHeuristicEstimatedSearch(index, cost + action.Cost, threshold);

                _dataSets[0].CopyTo(_dataSets[index]);

                if (!score.HasValue)
                {
                    _plan[depth] = action;
                    return null;
                }

                if (score.Value < min) min = score.Value;
            }
            return min;
        }
    }
}