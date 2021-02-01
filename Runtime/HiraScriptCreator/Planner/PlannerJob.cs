using System.Collections.Generic;
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

    public interface IActualAction
    {
        ActionData Data { get; }
    }

    [BurstCompile]
    public unsafe struct PlannerJob<T> : IJob where T : unmanaged, IBlackboard
    {
        [ReadOnly] private readonly int _datasetsLength;
        [NativeDisableUnsafePtrRestriction] private T* _datasetsPtr;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [DeallocateOnJobCompletion] private readonly NativeArray<T> _datasets;
        [ReadOnly] private readonly int _goal;
        [DeallocateOnJobCompletion] [ReadOnly] private readonly NativeArray<ActionData> _actions;
        [ReadOnly] private readonly int _actionsCount;
        [ReadOnly] private readonly float _maxFScore;
        [WriteOnly] public NativeArray<int> Plan;

        public PlannerJob(ref T dataset, int goal, int maxPlanLength, float maxFScore,
            IActualAction[] actions)
            : this(ref dataset, goal, maxPlanLength, maxFScore, ConvertToActionData(actions))
        {
            
        }

        public PlannerJob(ref T dataset, int goal, int maxPlanLength, float maxFScore,
            List<IActualAction> actions)
            : this(ref dataset, goal, maxPlanLength, maxFScore, ConvertToActionData(actions))
        {
            
        }
        
        public PlannerJob(ref T dataset, int goal, int maxPlanLength, float maxFScore,
            NativeArray<ActionData> actions)
        {
            _datasets = new NativeArray<T>(maxPlanLength + 1, Allocator.TempJob) {[0] = dataset};
            _datasetsPtr = (T*) _datasets.GetUnsafePtr();
            _datasetsLength = maxPlanLength + 1;
            _actions = actions;
            _actionsCount = actions.Length;
            _maxFScore = maxFScore;
            _goal = goal;
            Plan = new NativeArray<int>(maxPlanLength + 1, Allocator.TempJob);
        }
        
        public void Execute()
        {
            float threshold = _datasetsPtr->GetGoalHeuristic(_goal), score;
            while ((score = PerformHeuristicEstimatedSearch(1, 0, threshold)) > 0 && score < _maxFScore) threshold = score;
            _datasetsPtr = null;
        }
        
        private float PerformHeuristicEstimatedSearch(int index, float cost, float threshold)
        {
            var heuristic = (_datasetsPtr + index - 1)->GetGoalHeuristic(_goal);
            
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
                
                if (!(_datasetsPtr + index - 1)->GetActionApplicability(action.ArchetypeIndex)) continue;
                
                *(_datasetsPtr + index) = *(_datasetsPtr + index - 1);
                (_datasetsPtr + index)->ApplyActionEffect(action.ArchetypeIndex);
                
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

        private static NativeArray<ActionData> ConvertToActionData(IActualAction[] actions)
        {
            var length = actions.Length;
            var data = new NativeArray<ActionData>(length, Allocator.TempJob);
            for (var i = 0; i < length; i++)
            {
                data[i] = actions[i].Data;
            }

            return data;
        }

        private static NativeArray<ActionData> ConvertToActionData(List<IActualAction> actions)
        {
            var length = actions.Count;
            var data = new NativeArray<ActionData>(length, Allocator.TempJob);
            for (var i = 0; i < length; i++)
            {
                data[i] = actions[i].Data;
            }

            return data;
        }
    }
}