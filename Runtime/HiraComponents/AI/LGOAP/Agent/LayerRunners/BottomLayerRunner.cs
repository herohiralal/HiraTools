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
	public class BottomLayerRunner : IChildLayerRunner, IDisposable
	{
		public BottomLayerRunner(
			IParentLayerRunner parent,
			MonoBehaviour coroutineRunner,
			 IBlackboardComponent blackboard,
			IPlannerDomain domain,
			byte layerIndex,
			float maxFScore,
            RawBlackboardArrayWrapper plannerDatasets,
            byte maxPlanLength)
		{
			Parent = parent;
			_coroutineRunner = coroutineRunner;
			_blackboard = blackboard;
			_domain = domain.DomainData;
			_layerIndex = layerIndex;
			_maxFScore = maxFScore;

            _plannerDatasets = plannerDatasets;
            _result.First = new PlannerResult(maxPlanLength, Allocator.Persistent) {Count = 0};
            _result.Second = new PlannerResult(maxPlanLength, Allocator.Persistent) {Count = 0};
		}

        public void Dispose()
        {
            _result.First.Dispose();
            _result.Second.Dispose();
        }
		
		public readonly IParentLayerRunner Parent;
		public TaskRunner Runner { private get; set; }
		
		private readonly MonoBehaviour _coroutineRunner;
		private readonly IBlackboardComponent _blackboard;
		private readonly RawDomainData _domain;
		private readonly byte _layerIndex;
		private readonly float _maxFScore;

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
					UpdateDebuggerPlan();

				Runner.Debugger = value;
			}
		}
		
		public bool SelfAndAllChildrenIdle => _currentState == LayerState.Idle;
		public bool SelfOrAnyChildScheduled => _currentState == LayerState.PlannerScheduled;
		public bool SelfOrAnyChildRunning => _currentState == LayerState.PlannerRunning;

        private bool _ignoreScheduledPlannerRun = false;
        private bool _ignorePlannerResult = false;
        private bool _taskNeedsUpdate = true;

        public void IgnoreScheduledPlannerRunForSelfAndChild()
        {
            if (_currentState == LayerState.PlannerScheduled) _ignoreScheduledPlannerRun = true;
            else throw new Exception($"{nameof(IgnoreScheduledPlannerRunForSelfAndChild)} was called even though the planner was not scheduled.");
        }

        public void IgnorePlannerResultForSelfAndChild()
        {
            if (_currentState == LayerState.PlannerRunning) _ignorePlannerResult = true;
            else throw new Exception($"{nameof(IgnorePlannerResultForSelfAndChild)} was called even though the planner was not running.");
        }

		public void OnPlanRunnerFinished(bool success)
		{
			if (success)
			{
				_domain[_layerIndex].Break(out _, out var actions);
				actions[_result.First.CurrentIndex].Break(out _, out _, out var effect);
				unsafe
				{
					// this will directly modify the blackboard, and not broadcast any events
					// but that is exactly what we want, because the modification is intended
					effect.Execute((byte*) _blackboard.Data.GetUnsafePtr());
				}

				if (_result.First.CanMoveNext)
				{
					_result.First.MoveNext();
					Runner.UpdateTask(_result.First.CurrentElement);
					UpdateDebuggerPlanIndex();
				}
				else
				{
					// pretend like the planner is executing the first action,
					// so that MainPlannerJob can check if it can be reused.
					_result.First.RestartPlan();
					_taskNeedsUpdate = true;
					Parent.OnChildFinished();
				}
			}
			else
			{
				_result.First.InvalidatePlan();
				_taskNeedsUpdate = true;
				SchedulePlanner();
			}
		}
		
		public void SchedulePlanner() => _coroutineRunner.StartCoroutine(RunTaskPlanner());

		public MainPlannerJob CreateMainPlannerJob()
		{
			var output = new MainPlannerJob(
				_domain[_layerIndex],
				Parent.Result.First,
				_result.First,
				_maxFScore,
				_plannerDatasets.Unwrap(),
				_result.Second);
			_result.Flip();
			return output;
		}

		private IEnumerator RunTaskPlanner()
		{
			_currentState = LayerState.PlannerScheduled;
			yield return new WaitForEndOfFrame();
            if (_ignoreScheduledPlannerRun)
            {
                _ignoreScheduledPlannerRun = false;
	            _currentState = LayerState.Idle;
	            yield break;
            }
			_currentState = LayerState.PlannerRunning;

            _plannerDatasets.CopyFirstFrom(_blackboard);

			var jobHandle = CreateMainPlannerJob().Schedule();

			FirstUpdate.Catch(new JobCompletionDelegateHelper {TrackedJobHandle = jobHandle, OnCompletion = CollectResult}.Invoke);
		}

		public void CollectResult()
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
					throw new Exception("Task planner provided uninitialized output.");
				case PlannerResultType.Failure:
					throw new Exception("Task planner failed, possible reason: Max F-Score not high enough, or a parent planner failed.");
				case PlannerResultType.Unchanged:
					if (_taskNeedsUpdate)
					{
						_taskNeedsUpdate = false;
						Runner.UpdateTask(_result.First.CurrentElement);
						UpdateDebuggerPlan();
					}
					break;
				case PlannerResultType.Success:
					_taskNeedsUpdate = false;
					Runner.UpdateTask(_result.First.CurrentElement);
					UpdateDebuggerPlan();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
		private void UpdateDebuggerPlan()
		{
			try
			{
				Debugger?.UpdateCorePlan(_result.First);
			}
			catch
			{
				// ignored
			}
		}

		[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
		private void UpdateDebuggerPlanIndex()
		{
			try
			{
				Debugger?.IncrementActionIndex();
			}
			catch
			{
				// ignored
			}
		}
	}
}