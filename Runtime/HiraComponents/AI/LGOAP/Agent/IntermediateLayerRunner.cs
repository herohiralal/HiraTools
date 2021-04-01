using System;
using System.Collections;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
    public class IntermediateLayerRunner : IParentLayerRunner, IChildLayerRunner, IDisposable
    {
        public IParentLayerRunner Parent { get; set; }
        public IChildLayerRunner Child { get; set; }
        
        private MonoBehaviour _coroutineRunner;
        private IBlackboardComponent _blackboard;
        private IPlannerDomain _domain;
        private readonly byte _layerIndex;

        private RawBlackboardArrayWrapper _plannerDatasets;
        private float _maxFScore;

        private FlipFlopPool<PlannerResult> _result;
        public ref FlipFlopPool<PlannerResult> Result => ref _result;

        public void Dispose()
        {
            _coroutineRunner = null;
            _blackboard = null;
            _domain = null;
        }

        public void OnChildFinished(bool success)
        {
            if (success)
            {
                _domain.DomainData[_layerIndex].Break(out _, out var actions);
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

                    Parent.OnChildFinished(true);
                }
            }
            else
            {
                // todo: handle this
            }
        }

        public void SchedulePlanner() => _coroutineRunner.StartCoroutine(RunPlannerAtThisLayer());

        public MainPlannerJob CreateMainPlannerJob()
        {
            var output = new MainPlannerJob(_domain.DomainData[_layerIndex], Parent.Result.First, Result.First, _maxFScore, _plannerDatasets, _blackboard, Result.Second);
            Result.Flip();
            return output;
        }

        private struct ResultCatcherHelper
        {
            public JobHandle TrackedJobHandle;
            public IChildLayerRunner Runner;

            public void Invoke()
            {
                TrackedJobHandle.Complete();
                Runner.CollectResult();
            }
        }

        private IEnumerator RunPlannerAtThisLayer()
        {
            yield return new WaitForEndOfFrame();

            var currentJobHandle = CreateMainPlannerJob().Schedule();

            IChildLayerRunner current = this;
            while (current is IParentLayerRunner childLayerAsParent && (current = childLayerAsParent.Child) != null)
            {
                currentJobHandle = current.CreateMainPlannerJob().Schedule(currentJobHandle);
            }

            FirstUpdate.Catch(new ResultCatcherHelper {Runner = this, TrackedJobHandle = currentJobHandle}.Invoke);
        }

        public void CollectResult()
        {
            var currentResult = _result.First;
            switch (currentResult.ResultType)
            {
                case PlannerResultType.Uninitialized:
                    throw new Exception("Planner provided uninitialized output.");
                case PlannerResultType.Failure:
                    throw new Exception("Planner failed, possible reason: Max F-Score not high enough, or a parent planner failed.");
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