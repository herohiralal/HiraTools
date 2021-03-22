using HiraEngine.Components.Blackboard;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	[BurstCompile]
	public unsafe struct GoalCalculatorJob : IJob
	{
		public GoalCalculatorJob(NativeArray<byte> blackboard, byte* insistenceCalculators, byte currentGoal, PlannerResult result)
		{
			_blackboard = blackboard;
			_insistenceCalculators = insistenceCalculators;
			_currentGoal = currentGoal;
			Result = result;
		}
		
		[ReadOnly] private readonly NativeArray<byte> _blackboard; // persistent
		[ReadOnly] [NativeDisableUnsafePtrRestriction] private readonly byte* _insistenceCalculators; // persistent
		[ReadOnly] private readonly byte _currentGoal; // managed
		[WriteOnly] public PlannerResult Result; // reused
	
		public void Execute()
		{
			var blackboard = (byte*) _blackboard.GetUnsafeReadOnlyPtr();

			var goal = byte.MaxValue;
			var cachedInsistence = -1f;

			var count = *(_insistenceCalculators + sizeof(ushort));
			var stream = _insistenceCalculators + sizeof(ushort) + sizeof(byte);
			for (byte i = 0; i < count; i++, stream += *(ushort*) stream)
			{
				var currentInsistence = MemoryBatchHelpers.ExecuteScoreCalculatorsBlock(blackboard, stream);

				var foundBetterGoal = currentInsistence > cachedInsistence;

				goal = foundBetterGoal ? i : goal;
				cachedInsistence = foundBetterGoal ? currentInsistence : cachedInsistence;
			}

			if (goal == byte.MaxValue)
			{
				Result.ResultType = PlannerResultType.Failure;
				Result.Count = 0;
			}
			else if (_currentGoal == goal)
			{
				Result.ResultType = PlannerResultType.Unchanged;
				Result.Count = 1;
				Result[0] = goal;
			}
			else
			{
				Result.ResultType = PlannerResultType.Success;
				Result.Count = 1;
				Result[0] = goal;
			}
		}
	}
}