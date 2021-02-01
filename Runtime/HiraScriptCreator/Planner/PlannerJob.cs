namespace UnityEngine
{
    [Unity.Burst.BurstCompile]
    public readonly struct ActionData
    {
        [Unity.Collections.ReadOnly] public readonly int Identifier;
        [Unity.Collections.ReadOnly] public readonly int ArchetypeIndex;
        [Unity.Collections.ReadOnly] public readonly float Cost;
        
        public ActionData(int identifier, int archetypeIndex, float cost)
        {
            Identifier = identifier;
            ArchetypeIndex = archetypeIndex;
            Cost = cost;
        }
    }

    public interface IBlackboard
    {
        [Unity.Burst.BurstCompile]
        int GetHeuristic(int target);
        [Unity.Burst.BurstCompile]
        bool PreconditionCheck(int target);
        [Unity.Burst.BurstCompile]
        void ApplyEffect(int target);
    }
    
    [Unity.Burst.BurstCompile]
    public unsafe struct PlannerJob<T> : Unity.Jobs.IJob where T : unmanaged, IBlackboard
    {
        [Unity.Collections.ReadOnly] private readonly int _datasetsLength;
        [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestriction] private T* _datasetsPtr;
        [Unity.Collections.DeallocateOnJobCompletion] private readonly Unity.Collections.NativeArray<T> _datasets;
        [Unity.Collections.ReadOnly] private readonly int _goal;
        [Unity.Collections.DeallocateOnJobCompletion] [Unity.Collections.ReadOnly] private readonly Unity.Collections.NativeArray<ActionData> _actions;
        [Unity.Collections.ReadOnly] private readonly int _actionsCount;
        [Unity.Collections.ReadOnly] private readonly float _maxFScore;
        [Unity.Collections.WriteOnly] public Unity.Collections.NativeArray<int> Plan;
        
        public PlannerJob(T* dataset, int goal, int maxPlanLength, float maxFScore,
            Unity.Collections.NativeArray<ActionData> actions, Unity.Collections.NativeArray<int> plan)
        {
            _datasets = new Unity.Collections.NativeArray<T>(maxPlanLength + 1, Unity.Collections.Allocator.TempJob) {[0] = *dataset};
            _datasetsPtr = (T*) Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.GetUnsafePtr(_datasets);
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
                    Plan[index] = action.Identifier;
                    return -1;
                }
                
                min = Unity.Mathematics.math.min(score, min);
            }
            
            return min;
        }
    }
}