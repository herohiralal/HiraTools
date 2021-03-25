using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
    [BurstCompile]
    public unsafe struct MainPlannerJob : IJob
    {
        public MainPlannerJob(
            RawLayer layer,
            PlannerResult previousLayerResult, byte previousLayerResultIndex,
            // PlannerResult currentPlan, byte currentPlanIndex,
            float maxFScore,
            RawBlackboardArrayWrapper datasets, IBlackboardComponent blackboard,
            PlannerResult output)
        {
            // domain
            layer.Break(out _targets, out _actions);
            _goal = default;

            // layering
            _previousLayerResult = previousLayerResult;
            _previousLayerResultIndex = previousLayerResultIndex;

            // previous run
            // _currentPlan = currentPlan;
            // _currentPlanIndex = currentPlanIndex;

            // settings
            _maxFScore = maxFScore;

            // runtime data
            _datasets = datasets.Unwrap();
            UnsafeUtility.MemCpy(_datasets[0], blackboard.Data.GetUnsafeReadOnlyPtr(), _datasets.BlackboardSize);

            // output
            _output = output;
        }

        // domain
        [NativeDisableUnsafePtrRestriction] [ReadOnly] private readonly RawTargetsArray _targets;
        [NativeDisableUnsafePtrRestriction] [ReadOnly] private readonly RawActionsArray _actions;
        [NativeDisableUnsafePtrRestriction] private RawBlackboardDecoratorsArray _goal;

        // layering
        [ReadOnly] private readonly PlannerResult _previousLayerResult;
        [ReadOnly] private readonly byte _previousLayerResultIndex;

        // previous run
        // [ReadOnly] private readonly PlannerResult _currentPlan;
        // [ReadOnly] private readonly byte _currentPlanIndex;

        // settings
        [ReadOnly] private readonly float _maxFScore;

        // runtime data
        private RawBlackboardArray _datasets;

        // output
        [WriteOnly] private PlannerResult _output;

        public void Execute()
        {
            // if parent planner is uninitialized then mark self as failed
            if (_previousLayerResult.ResultType == PlannerResultType.Uninitialized)
            {
                Debug.LogError("Planner was run with the parent result still being uninitialized.");
                _output.ResultType = PlannerResultType.Failure;
                _output.Count = 0;
                return;
            }

            // if parent planner failed, no point in calculating anything
            if (_previousLayerResult.ResultType == PlannerResultType.Failure)
            {
                _output.ResultType = PlannerResultType.Failure;
                _output.Count = 0;
                return;
            }

            _goal = _targets[_previousLayerResult[_previousLayerResultIndex]];

            if (_previousLayerResult.ResultType == PlannerResultType.Unchanged)
            {
                // todo: determine if the previous goal is still applicable
            }

            float threshold = _goal.CalculateHeuristic(_datasets[0]);
            float score;
            while ((score = PerformHeuristicEstimatedSearch(1, 0, threshold)) > 0 && score < _maxFScore)
                threshold = score;
            _datasets = new RawBlackboardArray();
        }

        public float PerformHeuristicEstimatedSearch(byte index, float cost, float threshold)
        {
            var heuristic = _goal.CalculateHeuristic(_datasets[index - 1]);

            var fScore = cost + heuristic;
            if (fScore > threshold) return fScore;

            if (heuristic == 0)
            {
                _output.ResultType = PlannerResultType.Success;
                _output.Count = (byte) (index - 1);
                return -1;
            }

            if (index == _datasets.Count) return float.MaxValue;

            var min = float.MaxValue;

            var iterator = new RawActionsArrayIterator(_actions);
            while (iterator.MoveNext)
            {
                iterator.Get(out var precondition, out var costCalculator, out var effect, out var i);

                if (!precondition.Execute(_datasets[index - 1])) continue;

                var currentCost = cost + costCalculator.Execute(_datasets[index - 1]);

                UnsafeUtility.MemCpy(_datasets[index], _datasets[index - 1], _datasets.BlackboardSize);

                effect.Execute(_datasets[index]);

                float score;
                if ((score = PerformHeuristicEstimatedSearch((byte) (index + 1), currentCost, threshold)) < 0)
                {
                    _output[(byte) (index - 1)] = i;
                    return -1;
                }

                min = math.min(score, min);
            }

            return min;
        }
    }
}