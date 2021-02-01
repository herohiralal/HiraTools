using Unity.Burst;

namespace UnityEngine
{
    public interface IBlackboard
    {
        [BurstCompile] bool GetGoalValidity(int target);
        [BurstCompile] int GetGoalHeuristic(int target);
        [BurstCompile] bool GetActionApplicability(int target);
        [BurstCompile] void ApplyActionEffect(int target);
    }
}