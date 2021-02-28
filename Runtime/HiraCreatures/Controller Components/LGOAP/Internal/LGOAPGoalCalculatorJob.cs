using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace HiraCreatures.Components.LGOAP.Internal
{
	[BurstCompile]
	public unsafe struct LGOAPGoalCalculatorJob<T> : IJob where T : unmanaged
	{
		[ReadOnly] private T _blackboard;
		[DeallocateOnJobCompletion] [ReadOnly] private NativeArray<LGOAPGoalData> _goals;
		[WriteOnly] private NativeArray<int> _currentGoalIndex;

		public LGOAPGoalCalculatorJob(ref T blackboard, NativeArray<LGOAPGoalData> goals)
		{
			_blackboard = blackboard;
			_goals = goals;
			_currentGoalIndex = new NativeArray<int>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
		}
		
		public void Execute()
		{
			fixed (T* blackboard = &_blackboard)
			{
				var length = _goals.Length;
				var data = (LGOAPGoalData*) _goals.GetUnsafeReadOnlyPtr();

				var goal = -1;
				var cachedInsistence = -1f;

				for (var i = 0; i < length; i++)
				{
					data += i;

					var currentInsistence = data->Insistence;
					var foundBetterGoal = data->IsValidOn.Invoke(blackboard) != 0 && currentInsistence > cachedInsistence;

					goal = foundBetterGoal ? i : goal;
					cachedInsistence = foundBetterGoal ? currentInsistence : cachedInsistence;
				}

				_currentGoalIndex[0] = goal;
			}
		}
	}
}