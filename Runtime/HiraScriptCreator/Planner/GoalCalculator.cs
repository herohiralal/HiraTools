using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine
{
    [BurstCompile]
    public readonly struct GoalData
    {
        [ReadOnly] public readonly int ArchetypeIndex;
        [ReadOnly] public readonly float Insistence;
        
        public GoalData(int archetypeIndex, float insistence)
        {
            ArchetypeIndex = archetypeIndex;
            Insistence = insistence;
        }
    }

    public static class GoalCalculator
    {
        public static unsafe int GetGoal<T>(T* blackboard, NativeArray<GoalData> goalData) where T : unmanaged, IBlackboard
        {
            var length = goalData.Length;
            var data = (GoalData*) goalData.GetUnsafePtr();
            
            var goal = -1;
            var cachedInsistence = -1f;
            
            for (var i = 0; i < length; i++)
            {
                data += i;

                var currentInsistence = data->Insistence;
                var foundBetterGoal = blackboard->GetGoalValidity(data->ArchetypeIndex) && currentInsistence > cachedInsistence;

                goal = foundBetterGoal ? i : goal;
                cachedInsistence = foundBetterGoal ? currentInsistence : cachedInsistence;
            }

            goalData.Dispose();
            
            return goal;
        }
    }
}