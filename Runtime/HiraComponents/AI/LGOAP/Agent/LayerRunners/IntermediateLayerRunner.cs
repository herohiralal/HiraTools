using System;
using System.Collections;
using System.Diagnostics;
using HiraEngine.Components.Blackboard;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
    public class IntermediateLayerRunner : IParentLayerRunner, IChildLayerRunner, IDisposable
    {
	    public IntermediateLayerRunner(
		    IParentLayerRunner parent,
		    MonoBehaviour coroutineRunner,
		    IBlackboardComponent blackboard,
		    IPlannerDomain domain,
		    byte layerIndex,
		    float maxFScore,
            RawBlackboardArrayWrapper plannerDatasets,
            byte maxPlanLength)
	    {
		    _parent = parent;
		    _coroutineRunner = coroutineRunner;
		    _blackboard = blackboard;
		    _domain = domain;
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

        private readonly IParentLayerRunner _parent;
        public IChildLayerRunner Child { get; set; }
        
        private readonly MonoBehaviour _coroutineRunner;
        private readonly IBlackboardComponent _blackboard;
        private readonly IPlannerDomain _domain;
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
            using (new BlackboardComponentBroadcastDisabler())
                _domain.IntermediateLayers[_layerIndex][_result.First.CurrentIndex].Effect.ApplyTo(_blackboard);

			if (_result.First.CanMoveNext)
			{
				_result.First.MoveNext();
				Child.SchedulePlanner();
				UpdateDebuggerPlanIndex();
			}
			else
			{
				// pretend like the planner is executing the first action,
				// so that MainPlannerJob can check if it can be reused.
				_result.First.RestartPlan();

				_parent.OnChildFinished();
			}
        }

        public void SchedulePlanner() => _coroutineRunner.StartCoroutine(RunPlannerAtThisLayer());

        public MainPlannerJob CreateMainPlannerJob()
        {
            var output = new MainPlannerJob(
	            _domain.DomainData[_layerIndex],
	            _parent.Result.First,
	            _result.First,
	            _maxFScore,
	            _plannerDatasets.Unwrap(),
	            _result.Second);
            _result.Flip();
            return output;
        }

        private IEnumerator RunPlannerAtThisLayer()
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

            var currentJobHandle = CreateMainPlannerJob().Schedule();

            IChildLayerRunner current = this;
            while (current is IParentLayerRunner childLayerAsParent && (current = childLayerAsParent.Child) != null)
            {
                currentJobHandle = current.CreateMainPlannerJob().Schedule(currentJobHandle);
            }

            FirstUpdate.Catch(new JobCompletionDelegateHelper {TrackedJobHandle = currentJobHandle, OnCompletion = CollectResult}.Invoke);
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
                    throw new Exception($"Planner at index {_layerIndex} provided uninitialized output.");
                case PlannerResultType.Failure:
                    throw new Exception($"Planner at index {_layerIndex} failed, possible reason: Max F-Score not high enough, or a parent planner failed.");
                case PlannerResultType.Unchanged:
                    Child.CollectResult();
                    UpdateDebuggerPlan();
                    break;
                case PlannerResultType.Success:
                    Child.CollectResult();
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
		        Debugger?.UpdateIntermediatePlan(_layerIndex, _result.First);
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
		        Debugger?.IncrementIntermediateGoalIndex(_layerIndex);
	        }
	        catch
	        {
		        // ignored
	        }
        }
    }
}