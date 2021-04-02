using System;
using System.Collections;
using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
    public class IntermediateLayerRunner : IParentLayerRunner, IChildLayerRunner
    {
	    public IntermediateLayerRunner(
		    IParentLayerRunner parent,
		    MonoBehaviour coroutineRunner,
		    IBlackboardComponent blackboard,
		    IPlannerDomain domain,
		    byte layerIndex,
		    float maxFScore)
	    {
		    Parent = parent;
		    _coroutineRunner = coroutineRunner;
		    _blackboard = blackboard;
		    _domain = domain.DomainData;
		    _layerIndex = layerIndex;
		    _maxFScore = maxFScore;
	    }

	    public readonly IParentLayerRunner Parent;
        public IChildLayerRunner Child { get; set; }
        
        private readonly MonoBehaviour _coroutineRunner;
        private readonly IBlackboardComponent _blackboard;
        private readonly RawDomainData _domain;
        private readonly byte _layerIndex;
        private readonly float _maxFScore;

        public RawBlackboardArrayWrapper PlannerDatasets;

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
				Child.SchedulePlanner();
			}
			else
			{
				// pretend like the planner is executing the first action,
				// so that MainPlannerJob can check if it can be reused.
				_result.First.RestartPlan();

				Parent.OnChildFinished();
			}
        }

        public void SchedulePlanner() => _coroutineRunner.StartCoroutine(RunPlannerAtThisLayer());

        public MainPlannerJob CreateMainPlannerJob()
        {
            var output = new MainPlannerJob(
	            _domain[_layerIndex],
	            Parent.Result.First,
	            _result.First,
	            _maxFScore,
	            PlannerDatasets,
	            _blackboard,
	            _result.Second);
            _result.Flip();
            return output;
        }

        private IEnumerator RunPlannerAtThisLayer()
        {
			_currentState = LayerState.PlannerScheduled;
            yield return new WaitForEndOfFrame();
            if (_ignoreResultOnce)
            {
	            _ignoreResultOnce = false;
	            _currentState = LayerState.Idle;
	            yield break;
            }
			_currentState = LayerState.PlannerRunning;

            var currentJobHandle = CreateMainPlannerJob().Schedule();

            IChildLayerRunner current = this;
            do
            {
                currentJobHandle = current.CreateMainPlannerJob().Schedule(currentJobHandle);
            } while (current is IParentLayerRunner childLayerAsParent && (current = childLayerAsParent.Child) != null);

            FirstUpdate.Catch(new JobCompletionDelegateHelper {TrackedJobHandle = currentJobHandle, OnCompletion = CollectResult}.Invoke);
        }

        public void CollectResult()
        {
	        _currentState = LayerState.Idle;
	        if (_ignoreResultOnce)
	        {
		        _ignoreResultOnce = false;
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