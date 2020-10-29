using System.Threading;
using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class Planner<T> : IPlanner<T> where T : IAction
    {
        public Planner(IBlackboardValueAccessor valueAccessor, byte maxPlanLength)
        {
            _valueAccessor = valueAccessor;
            _dataSets = new IReadWriteBlackboardDataSet[maxPlanLength + 1];
            _dataSets[0] = valueAccessor.DataSet.GetDuplicate();
            for (var i = 1; i < maxPlanLength + 1; i++) _dataSets[i] = _dataSets[0].GetDuplicate();
            
            _plan = new T[maxPlanLength];
        }

        public bool IsActive { get; private set; } = false;

        private readonly IBlackboardValueAccessor _valueAccessor;
        private readonly IReadWriteBlackboardDataSet[] _dataSets;

        private IBlackboardQuery[] _goal = null;
        private T[] _actions = null;

        private float _maxFScore = 0f;
        private CancellationToken _ct = CancellationToken.None;
        private PlannerCompletionCallback<T> _onPlannerFinish = null;
        private readonly T[] _plan = null;
        private int _planLength = 0;
        private PlannerResult _result;

        public IPlanner<T> Initialize()
        {
            _valueAccessor.DataSet.CopyTo(_dataSets[0]);
            return this;
        }

        public IPlanner<T> ForGoal(IBlackboardQuery[] goal)
        {
            _goal = goal;
            return this;
        }

        public IPlanner<T> WithAvailableTransitions(T[] actions)
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
            _onPlannerFinish = completionCallback;
        }

        public void Run()
        {
            IsActive = true;
            GeneratePlan();
            ExecuteCallbackAndCleanup();
        }

        public void RunMultiThreaded()
        {
            IsActive = true;
            ThreadPool.QueueUserWorkItem(GeneratePlanMultiThreaded);
        }

        private void GeneratePlan(object obj = null)
        {
            float threshold = GetHeuristic(0);

            while (true)
            {
                var score = _ct.IsCancellationRequested
                    ? -1 
                    : PerformHeuristicEstimatedSearch(1, 0, threshold);

                if (!score.HasValue)
                {
                    _result = PlannerResult.Success;
                    break;
                }

                if (score.Value > _maxFScore)
                {
                    _result = PlannerResult.Failure;
                    break;
                }

                if (score.Value < 0)
                {
                    _result = PlannerResult.Cancelled;
                    break;
                }

                threshold = score.Value;
            }
        }

        private void GeneratePlanMultiThreaded(object obj = null)
        {
            GeneratePlan();
            MainThreadDispatcher.Schedule(ExecuteCallbackAndCleanup);
        }

        private void ExecuteCallbackAndCleanup()
        {
            var result = _ct.IsCancellationRequested ? PlannerResult.Cancelled : _result;
            var callback = _onPlannerFinish;

            _goal = null;
            _actions = null;
            _maxFScore = 0f;
            _onPlannerFinish = null;
            _ct = CancellationToken.None;
            _result = PlannerResult.None;
            IsActive = false;
            
            callback.Invoke(result, _plan, _planLength);
        }

        private int GetHeuristic(int index)
        {
            var (length, count) = (_goal.Length, 0);
            for (var i = 0; i < length; i++)
            {
                count += _goal[i].IsSatisfiedBy(_dataSets[index])
                    ? 0
                    : 1;
            }
            return count;
        }

        private float? PerformHeuristicEstimatedSearch(int index, float cost, float threshold)
        {
            var heuristic = GetHeuristic(index - 1);
            var fScore = cost + heuristic;
            if (fScore > threshold) return fScore;

            if (heuristic == 0)
            {
                _planLength = index - 1;
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