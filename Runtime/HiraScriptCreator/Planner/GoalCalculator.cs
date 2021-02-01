using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine
{
    [BurstCompile]
    public readonly struct GoalData : IActualGoal
    {
        [ReadOnly] public readonly int ArchetypeIndex;
        [ReadOnly] public readonly float Insistence;
        
        public GoalData(int archetypeIndex, float insistence)
        {
            ArchetypeIndex = archetypeIndex;
            Insistence = insistence;
        }

        public GoalData Data => this;
    }

    public interface IActualGoal
    {
        GoalData Data { get; }
    }

    public static class GoalCalculator
    {
        private static unsafe int GetGoal<T>(ref T blackboard, NativeArray<GoalData> goalData) where T : unmanaged, IBlackboard
        {
            var length = goalData.Length;
            var data = (GoalData*) goalData.GetUnsafePtr();
            
            var goal = -1;
            var cachedInsistence = -1f;
            
            for (var i = 0; i < length; i++)
            {
                data += i;

                var currentInsistence = data->Insistence;
                var foundBetterGoal = blackboard.GetGoalValidity(data->ArchetypeIndex) && currentInsistence > cachedInsistence;

                goal = foundBetterGoal ? i : goal;
                cachedInsistence = foundBetterGoal ? currentInsistence : cachedInsistence;
            }
            
            return goal;
        }

        public static TGoal GetGoal<TBlackboard, TGoal>(ref TBlackboard blackboard, TGoal[] goals)
            where TBlackboard : unmanaged, IBlackboard
            where TGoal : IActualGoal
        {
            var length = goals.Length;
            var data = new NativeArray<GoalData>(length, Allocator.Temp);
            for (var i = 0; i < length; i++)
            {
                data[i] = goals[i].Data;
            }
            
            var goal = GetGoal(ref blackboard, data);
            data.Dispose();
            return goals[goal];
        }

        public static TGoal GetGoal<TBlackboard, TGoal>(ref TBlackboard blackboard, List<TGoal> goals)
            where TBlackboard : unmanaged, IBlackboard
            where TGoal : IActualGoal
        {
            var length = goals.Count;
            var data = new NativeArray<GoalData>(length, Allocator.Temp);
            for (var i = 0; i < length; i++)
            {
                data[i] = goals[i].Data;
            }
            
            var goal = GetGoal(ref blackboard, data);
            data.Dispose();
            return goals[goal];
        }
    }
}