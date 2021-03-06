﻿using HiraEngine.Components.AI.LGOAP.Raw;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	[BurstCompile(FloatPrecision.Low, FloatMode.Fast, DisableSafetyChecks = true)]
	public unsafe struct GoalCalculatorJob : IJob
	{
        public GoalCalculatorJob(IBlackboardComponent blackboard, RawInsistenceCalculatorsArray domainData, PlannerResult result)
        {
            _blackboard = blackboard.Data;
            _insistenceCalculators = domainData;
            _result = result;
        }
        
        [ReadOnly] private readonly NativeArray<byte> _blackboard; // persistent
        [NativeDisableUnsafePtrRestriction] [ReadOnly] private readonly RawInsistenceCalculatorsArray _insistenceCalculators;
		private PlannerResult _result; // reused
	
		public void Execute()
		{
			var blackboard = (byte*) _blackboard.GetUnsafeReadOnlyPtr();

			var goal = byte.MaxValue;
			var cachedInsistence = -1f;

            var iterator = new RawInsistenceCalculatorsArrayIterator(_insistenceCalculators);
            while (iterator.MoveNext)
            {
                iterator.Get(out var insistenceCalculator, out var index);

                var currentInsistence = insistenceCalculator.Execute(blackboard);

                var foundBetterGoal = currentInsistence > cachedInsistence;

                goal = foundBetterGoal ? index : goal;
                cachedInsistence = foundBetterGoal ? currentInsistence : cachedInsistence;
            }

			if (goal == byte.MaxValue)
			{
				_result.ResultType = PlannerResultType.Failure;
				_result.Count = 0;
			}
			else if (_result.CurrentIndex == goal)
			{
				_result.ResultType = PlannerResultType.Unchanged;
                _result.CurrentIndex = 0;
				_result.Count = 1;
				_result[0] = goal;
			}
			else
			{
				_result.ResultType = PlannerResultType.Success;
                _result.CurrentIndex = 0;
				_result.Count = 1;
				_result[0] = goal;
			}
		}
	}
}