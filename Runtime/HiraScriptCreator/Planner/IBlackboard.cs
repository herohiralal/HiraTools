using Unity.Burst;

namespace UnityEngine
{
    public interface IBlackboard
    {
        [BurstCompile] int GetHeuristic(int target);
        [BurstCompile] bool PreconditionCheck(int target);
        [BurstCompile] void ApplyEffect(int target);
    }
}