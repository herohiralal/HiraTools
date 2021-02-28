using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace HiraCreatures.Components.LGOAP.Internal
{
	[BurstCompile]
	public unsafe struct LGOAPlannerJob<T> : IJob where T : unmanaged
	{
		[ReadOnly] private readonly int _datasetsLength;
		[NativeDisableUnsafePtrRestriction] private T* _datasetsPtr;

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		[DeallocateOnJobCompletion] private readonly NativeArray<T> _datasets;
		[ReadOnly] private readonly FunctionPointer<LGOAPGoalHeuristicCheckDelegate> _goal;
		[DeallocateOnJobCompletion] [ReadOnly] private readonly NativeArray<LGOAPActionData> _actions;
		[ReadOnly] private readonly int _actionsCount;
		[ReadOnly] private readonly float _maxFScore;
		[WriteOnly] public NativeArray<int> Plan;

		public LGOAPlannerJob(ref T dataset, FunctionPointer<LGOAPGoalHeuristicCheckDelegate> goal, int maxPlanLength, float maxFScore,
			NativeArray<LGOAPActionData> actions)
		{
			_datasets = new NativeArray<T>(maxPlanLength + 1, Allocator.TempJob) {[0] = dataset};
			_datasetsPtr = (T*) _datasets.GetUnsafePtr();
			_datasetsLength = maxPlanLength + 1;
			_actions = actions;
			_actionsCount = actions.Length;
			_maxFScore = maxFScore;
			_goal = goal;
			Plan = new NativeArray<int>(maxPlanLength + 1, Allocator.TempJob);
		}

		public void Execute()
		{
			float threshold = _goal.Invoke(_datasetsPtr), score;
			while ((score = PerformHeuristicEstimatedSearch(1, 0, threshold)) > 0 && score < _maxFScore) threshold = score;
			_datasetsPtr = null;
		}

		private float PerformHeuristicEstimatedSearch(int index, float cost, float threshold)
		{
			var heuristic = _goal.Invoke(_datasetsPtr + index - 1);

			var fScore = cost + heuristic;
			if (fScore > threshold) return fScore;

			if (heuristic == 0)
			{
				Plan[0] = index - 1;
				return -1;
			}

			if (index == _datasetsLength) return float.MaxValue;

			var min = float.MaxValue;

			for (var i = 0; i < _actionsCount; i++)
			{
				var action = _actions[i];

				if (action.IsApplicableTo.Invoke(_datasetsPtr) == 0) continue;

				*(_datasetsPtr + index) = *(_datasetsPtr + index - 1);
				action.ApplyTo.Invoke(_datasetsPtr + index);

				float score;
				if ((score = PerformHeuristicEstimatedSearch(index + 1, cost + action.Cost, threshold)) < 0)
				{
					Plan[index] = i;
					return -1;
				}

				min = math.min(score, min);
			}

			return min;
		}
	}
}