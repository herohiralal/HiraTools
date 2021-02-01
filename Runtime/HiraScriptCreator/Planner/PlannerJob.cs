using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine
{
    [BurstCompile]
    public readonly struct ActionData
    {
        [ReadOnly] public readonly int ArchetypeIndex;
        [ReadOnly] public readonly float Cost;
        
        public ActionData(int archetypeIndex, float cost)
        {
            ArchetypeIndex = archetypeIndex;
            Cost = cost;
        }
    }

    [BurstCompile]
    public unsafe struct PlannerJob<T> : IJob where T : unmanaged, IBlackboard
    {
        [ReadOnly] private readonly int _datasetsLength;
        [NativeDisableUnsafePtrRestriction] private T* _datasetsPtr;
        [DeallocateOnJobCompletion] private readonly NativeArray<T> _datasets;
        [ReadOnly] private readonly int _goal;
        [DeallocateOnJobCompletion] [ReadOnly] private readonly NativeArray<ActionData> _actions;
        [ReadOnly] private readonly int _actionsCount;
        [ReadOnly] private readonly float _maxFScore;
        [WriteOnly] public NativeArray<int> Plan;
        
        public PlannerJob(T* dataset, int goal, int maxPlanLength, float maxFScore,
            NativeArray<ActionData> actions, NativeArray<int> plan)
        {
            _datasets = new NativeArray<T>(maxPlanLength + 1, Allocator.TempJob) {[0] = *dataset};
            _datasetsPtr = (T*) _datasets.GetUnsafePtr();
            _datasetsLength = maxPlanLength + 1;
            _actions = actions;
            _actionsCount = actions.Length;
            _maxFScore = maxFScore;
            _goal = goal;
            Plan = plan;
        }
        
        public void Execute()
        {
            float threshold = _datasetsPtr->GetHeuristic(_goal), score;
            while ((score = PerformHeuristicEstimatedSearch(1, 0, threshold)) > 0 && score < _maxFScore) threshold = score;
            _datasetsPtr = null;
        }
        
        private float PerformHeuristicEstimatedSearch(int index, float cost, float threshold)
        {
            var heuristic = (_datasetsPtr + index - 1)->GetHeuristic(_goal);
            
            var fScore = cost + heuristic;
            if (fScore > threshold) return fScore;
            
            if (heuristic == 0)
            {
                Plan[0] = index - 1;
                return -1;
            }
            
            if (index == _datasetsLength) return float.MaxValue;
            
            var min = float.MaxValue;
            
            for (var i = 0; i < _actionsCount; i++)
            {
                var action = _actions[i];
                
                if (!(_datasetsPtr + index - 1)->PreconditionCheck(action.ArchetypeIndex)) continue;
                
                *(_datasetsPtr + index) = *(_datasetsPtr + index - 1);
                (_datasetsPtr + index)->ApplyEffect(action.ArchetypeIndex);
                
                float score;
                if ((score = PerformHeuristicEstimatedSearch(index + 1, cost + action.Cost, threshold)) < 0)
                {
                    Plan[index] = i;
                    return -1;
                }
                
                min = math.min(score, min);
            }
            
            return min;
        }
    }
}