using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
    [BurstCompile]
    public unsafe struct MainPlannerJob : IJob
    {
        public MainPlannerJob(RawLayer layer, PlannerResult previousLayerResult, byte previousLayerResultIndex, byte* datasets, PlannerResult output)
        {
            layer.Break(out _targets, out _actions);
            _target = default;
            _previousLayerResult = previousLayerResult;
            _previousLayerResultIndex = previousLayerResultIndex;
            _datasets = datasets;
            _output = output;
        }
        
        // domain
        [NativeDisableUnsafePtrRestriction] [ReadOnly] private readonly RawTargetsArray _targets;
        [NativeDisableUnsafePtrRestriction] [ReadOnly] private readonly RawActionsArray _actions;
        [NativeDisableUnsafePtrRestriction] private RawBlackboardDecoratorsArray _target;
        
        // layering
        [ReadOnly] private readonly PlannerResult _previousLayerResult;
        [ReadOnly] private readonly byte _previousLayerResultIndex;
        
        // previous run
        // [ReadOnly] private readonly PlannerResult _currentPlan;
        // [ReadOnly] private readonly byte _currentPlanIndex;
        
        // settings
        // [ReadOnly] private readonly float _maxFScore;
        
        // runtime data
        [NativeDisableUnsafePtrRestriction] [ReadOnly] private readonly byte* _datasets;
        // [ReadOnly] private readonly ushort _datasetLength;
        // [ReadOnly] private readonly byte _datasetsCount;
        
        // output
        [WriteOnly] private PlannerResult _output;
        
        public void Execute()
        {
            // if parent planner is uninitialized then mark self as failed
            if (_previousLayerResult.ResultType == PlannerResultType.Uninitialized)
            {
                Debug.LogError("Planner was run with the parent result still being uninitialized.");
                _output.ResultType = PlannerResultType.Failure;
                _output.Count = 0;
                return;
            }

            // if parent planner failed, no point in calculating anything
            if (_previousLayerResult.ResultType == PlannerResultType.Failure)
            {
                _output.ResultType = PlannerResultType.Failure;
                _output.Count = 0;
                return;
            }

            _target = _targets[_previousLayerResult[_previousLayerResultIndex]];
            
            _output.ResultType = PlannerResultType.Success;
            _output.Count = 1;

            _output[0] = _target.Execute(_datasets) ? (byte) 1 : (byte) 0;
        }
    }
}