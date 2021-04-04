using System;
using System.Collections;
using System.Diagnostics;
using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public class TopLayerRunner : IParentLayerRunner, IDisposable
	{
		public TopLayerRunner(
			MonoBehaviour coroutineRunner,
			IBlackboardComponent blackboard,
			IPlannerDomain domain,
			RawBlackboardArrayWrapper plannerDatasets)
		{
			_coroutineRunner = coroutineRunner;
			_blackboard = blackboard;
			_domain = domain.DomainData;

			_plannerDatasets = plannerDatasets;
            _result.First = new PlannerResult(1, Allocator.Persistent) {Count = 1, [0] = byte.MaxValue};
            _result.Second = new PlannerResult(1, Allocator.Persistent) {Count = 1, [0] = byte.MaxValue};
		}

        public void Dispose()
        {
            _result.First.Dispose();
            _result.Second.Dispose();
        }
		
		public IChildLayerRunner Child { get; set; }

		private readonly MonoBehaviour _coroutineRunner;
		private readonly IBlackboardComponent _blackboard;
		private readonly RawDomainData _domain;

        private RawBlackboardArrayWrapper _plannerDatasets;

		private FlipFlopPool<PlannerResult> _result;
		public ref FlipFlopPool<PlannerResult> Result => ref _result;

		private LayerState _currentState = LayerState.Idle;

		private IPlannerDebugger _debugger = null;

		public IPlannerDebugger Debugger
		{
			get => _debugger;
			set
			{
				_debugger = value;
				if (_result.First.ResultType == PlannerResultType.Success || _result.First.ResultType == PlannerResultType.Unchanged)
					UpdateDebugger(_result.First[0]);

				Child.Debugger = value;
			}
		}

		public bool SelfAndAllChildrenIdle => _currentState == LayerState.Idle && Child.SelfAndAllChildrenIdle;
		public bool SelfOrAnyChildScheduled => _currentState == LayerState.PlannerScheduled || Child.SelfOrAnyChildScheduled;
		public bool SelfOrAnyChildRunning => _currentState == LayerState.PlannerRunning || Child.SelfOrAnyChildRunning;

        private bool _ignoreScheduledPlannerRun = false;
        private bool _ignorePlannerResult = false;

        public void IgnoreScheduledPlannerRunForSelfAndChild()
        {
            if (_currentState == LayerState.PlannerScheduled) _ignoreScheduledPlannerRun = true;
            else Child.IgnoreScheduledPlannerRunForSelfAndChild();
        }

        public void IgnorePlannerResultForSelfAndChild()
        {
            if (_currentState == LayerState.PlannerRunning) _ignorePlannerResult = true;
            else Child.IgnorePlannerResultForSelfAndChild();
        }

		public void OnChildFinished()
		{
			unsafe
			{
				// this will directly modify the blackboard, and not broadcast any events
				// but that is exactly what we want, because the modification is intended
				_domain.Restarters.Execute((byte*) _blackboard.Data.GetUnsafePtr());
			}

			SchedulePlanner();
		}

		public void SchedulePlanner() => _coroutineRunner.StartCoroutine(RunFullPlanner());

		private GoalCalculatorJob CreateGoalCalculatorJob()
		{
			var goalResult = _result.Second;
			goalResult.CurrentIndex = _result.First[0];

			var output = new GoalCalculatorJob(
				_blackboard,
				_domain.InsistenceCalculators,
				goalResult);
			_result.Flip();
			return output;
		}

		private IEnumerator RunFullPlanner()
		{
			_currentState = LayerState.PlannerScheduled;

            if (Child.SelfOrAnyChildRunning)
            {
                Child.IgnorePlannerResultForSelfAndChild();
                while (Child.SelfOrAnyChildRunning) yield return null;
            }

            if (Child.SelfOrAnyChildScheduled)
                Child.IgnoreScheduledPlannerRunForSelfAndChild();
			
			yield return new WaitForEndOfFrame();
            if (_ignoreScheduledPlannerRun)
            {
                _ignoreScheduledPlannerRun = false;
	            _currentState = LayerState.Idle;
	            yield break;
            }
            
			_currentState = LayerState.PlannerRunning;

            _plannerDatasets.CopyFirstFrom(_blackboard);

			var currentJobHandle = CreateGoalCalculatorJob().Schedule();

			var current = Child;
			do
			{
				currentJobHandle = current.CreateMainPlannerJob().Schedule(currentJobHandle);
			} while (current is IParentLayerRunner childLayerAsParent && (current = childLayerAsParent.Child) != null);

			FirstUpdate.Catch(new JobCompletionDelegateHelper {TrackedJobHandle = currentJobHandle, OnCompletion = CollectResult}.Invoke);
		}

		private void CollectResult()
		{
			_currentState = LayerState.Idle;
	        if (_ignorePlannerResult)
	        {
                _ignorePlannerResult = false;
		        return;
	        }
			var currentResult = _result.First;
			switch (currentResult.ResultType)
			{
				case PlannerResultType.Uninitialized:
					throw new Exception("Goal calculator provided uninitialized output.");
				case PlannerResultType.Failure:
					throw new Exception("Goal calculator failed.");
				case PlannerResultType.Unchanged:
					Child.CollectResult();
					UpdateDebugger(currentResult[0]);
					break;
				case PlannerResultType.Success:
					Child.CollectResult();
					UpdateDebugger(currentResult[0]);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
		private void UpdateDebugger(byte goalIndex)
		{
			try
			{
				Debugger?.UpdateGoal(goalIndex);
			}
			catch
			{
				// ignored
			}
		}
	}
}