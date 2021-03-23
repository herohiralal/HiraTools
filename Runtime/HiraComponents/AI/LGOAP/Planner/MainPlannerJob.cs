using HiraEngine.Components.Blackboard;
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
        public MainPlannerJob(byte* layerData, PlannerResult previousLayerResult, byte previousLayerResultIndex, byte* datasets, PlannerResult output)
        {
            _layerData = layerData;
            _target = null;
            _previousLayerResult = previousLayerResult;
            _previousLayerResultIndex = previousLayerResultIndex;
            _datasets = datasets;
            _output = output;
        }
        
        // domain
        [NativeDisableUnsafePtrRestriction] [ReadOnly] private readonly byte* _layerData;
        [NativeDisableUnsafePtrRestriction] private byte* _target;
        
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

            if (!SelectTarget())
            {
                _output.ResultType = PlannerResultType.Failure;
                _output.Count = 0;
                return;
            }
            
            _output.ResultType = PlannerResultType.Success;
            _output.Count = 1;

            _output[0] = MemoryBatchHelpers.ExecuteDecoratorsBlock(_datasets, _target) ? (byte) 1 : (byte) 0;
        }

        private bool SelectTarget()
        {
            var targetsBlockAddress = _layerData + sizeof(ushort); // size header of this block

            var count = *(targetsBlockAddress++); // number of targets
            var indexOfTarget = _previousLayerResult[_previousLayerResultIndex];
            if (indexOfTarget >= count)
            {
                Debug.LogError($"Previous layer spat out an invalid index. Count: {count}, Index: {indexOfTarget}.");
                return false;
            }
            
            for (var i = 0; i < indexOfTarget; i++)
            {
                targetsBlockAddress += *targetsBlockAddress;
            }

            _target = targetsBlockAddress;
            return true;
        }
    }
}