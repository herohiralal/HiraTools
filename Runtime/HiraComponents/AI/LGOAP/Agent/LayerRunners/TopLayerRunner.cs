using System;
using System.Collections;
using HiraEngine.Components.AI.LGOAP.Raw;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public class TopLayerRunner : IParentLayerRunner
	{
		public TopLayerRunner(
			MonoBehaviour coroutineRunner,
			IBlackboardComponent blackboard,
			IPlannerDomain domain)
		{
			_coroutineRunner = coroutineRunner;
			_blackboard = blackboard;
			_domain = domain.DomainData;
		}
		
		public IChildLayerRunner Child { get; set; }

		private readonly MonoBehaviour _coroutineRunner;
		private readonly IBlackboardComponent _blackboard;
		private readonly RawDomainData _domain;

		private FlipFlopPool<PlannerResult> _result;
		public ref FlipFlopPool<PlannerResult> Result => ref _result;

		private LayerState _currentState = LayerState.Idle;

		public bool SelfAndAllChildrenIdle => _currentState == LayerState.Idle && Child.SelfAndAllChildrenIdle;
		public bool SelfOrAnyChildScheduled => _currentState == LayerState.PlannerScheduled || Child.SelfOrAnyChildScheduled;
		public bool SelfOrAnyChildRunning => _currentState == LayerState.PlannerRunning || Child.SelfOrAnyChildRunning;

		private bool _ignoreResultOnce = false;
		public void IgnoreResultOnce()
		{
			switch (_currentState)
			{
				case LayerState.Idle:
					Child.IgnoreResultOnce();
					break;
				case LayerState.PlannerScheduled:
					_ignoreResultOnce = true;
					break;
				case LayerState.PlannerRunning:
					_ignoreResultOnce = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
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

			if (Child.SelfOrAnyChildScheduled)
			{
				
			}
			
			yield return new WaitForEndOfFrame();
			if (_ignoreResultOnce)
			{
				_ignoreResultOnce = false;
				_currentState = LayerState.Idle;
				yield break;
			}
			_currentState = LayerState.PlannerRunning;

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
			var currentResult = _result.First;
			switch (currentResult.ResultType)
			{
				case PlannerResultType.Uninitialized:
					throw new Exception("Goal calculator provided uninitialized output.");
				case PlannerResultType.Failure:
					throw new Exception("Goal calculator failed.");
				case PlannerResultType.Unchanged:
					Child.CollectResult();
					break;
				case PlannerResultType.Success:
					Child.CollectResult();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}