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
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast, DisableSafetyChecks = true)]
	public unsafe struct MainPlannerJob : IJob
	{
		public MainPlannerJob(
			RawLayer layer,
			PlannerResult previousLayerResult,
			PlannerResult currentPlan,
			float maxFScore,
			RawBlackboardArrayWrapper datasets, IBlackboardComponent blackboard,
			PlannerResult output)
		{
			// domain
			layer.Break(out _targets, out _actions);
			_goal = default;

			// layering
			_previousLayerResult = previousLayerResult;

			// previous run
			_currentPlan = currentPlan;

			// settings
			_maxFScore = maxFScore;

			// runtime data
			_datasets = datasets.Unwrap();
			UnsafeUtility.MemCpy(_datasets[0], blackboard.Data.GetUnsafeReadOnlyPtr(), _datasets.BlackboardSize);

			// output
			_output = output;
		}

		// domain
		[NativeDisableUnsafePtrRestriction] [ReadOnly]
		private readonly RawTargetsArray _targets;
		[NativeDisableUnsafePtrRestriction] [ReadOnly]
		private readonly RawActionsArray _actions;
		[NativeDisableUnsafePtrRestriction] private RawBlackboardDecoratorsArray _goal;

		// layering
		[ReadOnly] private readonly PlannerResult _previousLayerResult;

		// previous run
		[ReadOnly] private readonly PlannerResult _currentPlan;

		// settings
		[ReadOnly] private readonly float _maxFScore;

		// runtime data
		private RawBlackboardArray _datasets;

		// output
		[WriteOnly] private PlannerResult _output;

		public void Execute()
		{
			var previousLayerResult = _previousLayerResult.ResultType;
			// if parent planner is uninitialized then mark self as failed
			if (previousLayerResult == PlannerResultType.Uninitialized)
			{
				Debug.LogError("Planner was run with the parent result still being uninitialized.");
				_output.ResultType = PlannerResultType.Failure;
				_output.Count = 0;
				return;
			}

			// if parent planner failed, no point in calculating anything
			if (previousLayerResult == PlannerResultType.Failure)
			{
				_output.ResultType = PlannerResultType.Failure;
				_output.Count = 0;
				return;
			}

			_goal = _targets[_previousLayerResult[_previousLayerResult.CurrentIndex]];

			if (previousLayerResult == PlannerResultType.Unchanged && CurrentPlanIsStillValid())
			{
				_output.Container.CopyFrom(_currentPlan.Container);
				_output.ResultType = PlannerResultType.Unchanged;
				return;
			}

			float threshold = _goal.CalculateHeuristic(_datasets[0]);
			float score;
			while ((score = PerformHeuristicEstimatedSearch(1, 0, threshold)) > 0 && score <= _maxFScore)
				threshold = score;
			_datasets = new RawBlackboardArray();

			if (score > _maxFScore)
			{
				_output.ResultType = PlannerResultType.Failure;
				_output.Count = 0;
			}
		}

		private bool CurrentPlanIsStillValid()
		{
			var currentPlanResult = _currentPlan.ResultType;

			if (currentPlanResult != PlannerResultType.Success && currentPlanResult != PlannerResultType.Unchanged)
				return false;

			UnsafeUtility.MemCpy(_datasets[1], _datasets[0], _datasets.BlackboardSize);

			var count = _currentPlan.Count;
			for (var i = _currentPlan.CurrentIndex; i < count; i++)
			{
				_actions[i].Break(out var precondition, out _, out var effect);

				if (precondition.Execute(_datasets[1]))
					effect.Execute(_datasets[1]);
				else return false;
			}

			return _goal.Execute(_datasets[1]);
		}

		private float PerformHeuristicEstimatedSearch(byte index, float cost, float threshold)
		{
			var heuristic = _goal.CalculateHeuristic(_datasets[index - 1]);

			var fScore = cost + heuristic;
			if (fScore > threshold) return fScore;

			if (heuristic == 0)
			{
				_output.ResultType = PlannerResultType.Success;
				_output.Count = (byte) (index - 1);
				_output.CurrentIndex = 0;
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