using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class Planner<T> : IPlanner<T> where T : IAction
    {
        public Planner(IBlackboardValueAccessor valueAccessor)
        {
            _valueAccessor = valueAccessor;
            _dataSet = valueAccessor.DataSet.GetDuplicate();
        }
        
        private readonly ThreadSafeObject<bool> _isActive = new ThreadSafeObject<bool>(false);
        public bool IsActive => _isActive.Value;

        private readonly IBlackboardValueAccessor _valueAccessor;
        private readonly IReadWriteBlackboardDataSet _dataSet;
        
        private IEnumerable<IBlackboardQuery> _goal = null;
        private IEnumerable<T> _actions = null;

        private float _maxFScore = 0f;
        private CancellationToken _ct = CancellationToken.None;
        private event PlannerCompletionCallback<T> OnPlannerFinish = delegate { };
        private T[] _plan = null;

        public IPlanner<T> Initialize()
        {
            _valueAccessor.DataSet.CopyTo(_dataSet);
            return this;
        }

        public IPlanner<T> ForGoal(IEnumerable<IBlackboardQuery> goal)
        {
            _goal = goal;
            return this;
        }

        public IPlanner<T> WithAvailableTransitions(IEnumerable<T> transitions)
        {
            _actions = transitions;
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

            float threshold = Heuristic;

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
            OnPlannerFinish = delegate { };
            _isActive.Value = false;
        }

        private int Heuristic => _goal.Count(_dataSet.DoesNotSatisfy);

        private float? PerformHeuristicEstimatedSearch(int depth, float cost, float threshold)
        {
            if (_ct.IsCancellationRequested) return -1;
            
            var heuristic = Heuristic;
            var fScore = cost + heuristic;
            if (fScore > threshold) return fScore;

            if (heuristic == 0)
            {
                _plan = new T[depth];
                return null;
            }

            var min = float.MaxValue;
            foreach (var action in _actions)
            {
                if (action.Preconditions.IsNotSatisfiedBy(_dataSet)) continue;

                var undo = action.Effects.ApplyTo(_dataSet);

                var score = PerformHeuristicEstimatedSearch(depth + 1, cost + action.Cost, threshold);

                undo.ApplyTo(_dataSet);

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