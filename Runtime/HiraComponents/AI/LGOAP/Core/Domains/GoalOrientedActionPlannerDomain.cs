using System;
using System.Linq;
using HiraEngine.Components.AI.LGOAP.Internal;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Assertions;

namespace HiraEngine.Components.AI.LGOAP
{
    [CreateAssetMenu(menuName = "Hiralal/AI/GOAP Domain", fileName = "New GOAPDomain")]
    [HiraCollectionCustomizer(1, MaxObjectCount = byte.MaxValue, Title = "Goals")]
    [HiraCollectionCustomizer(2, MaxObjectCount = byte.MaxValue, Title = "Actions")]
    public unsafe class GoalOrientedActionPlannerDomain : HiraCollection<Goal, Action>, IInitializable
    {
        [NonSerialized] private NativeArray<byte> _domainData = default;

        public void Initialize<T>(ref T initParams)
        {
            ushort size = 0;
            
            var insistenceCalculators = Collection1.Select(g=>g.Collection1).ToArray();
            var targets = Collection1.Select(g => g.Collection2).ToArray();
            var actions = Collection2.Select(a => (a.Collection1, a.Collection2, a.Collection3)).ToArray();

            var insistenceCalculatorsBlockSize = insistenceCalculators.GetInsistenceCalculatorsBlockSize();
            size += insistenceCalculatorsBlockSize;
            
            var layerBlockSize = LayerMemoryBatchHelpers.GetLayerMemorySize(targets, actions);
            size += layerBlockSize;

            _domainData = new NativeArray<byte>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var stream = (byte*) _domainData.GetUnsafePtr();

            var insistenceCalculatorsAllocatedBlockSize = insistenceCalculators.AppendInsistenceCalculatorsBlock(stream);
            Assert.AreEqual(insistenceCalculatorsBlockSize, insistenceCalculatorsAllocatedBlockSize);
            stream += insistenceCalculatorsAllocatedBlockSize;

            var layerAllocatedBlockSize = LayerMemoryBatchHelpers.AppendEntireLayerBlock(targets, actions, stream);
            Assert.AreEqual(layerBlockSize, layerAllocatedBlockSize);
        }

        public void Shutdown()
        {
            _domainData.Dispose();
        }

        public JobHandle ScheduleGoalCalculatorJob(HiraBlackboardComponent blackboard, byte previousResult, PlannerResult result)
        {
	        var job = new GoalCalculatorJob(blackboard.Data, (byte*) _domainData.GetUnsafeReadOnlyPtr(), previousResult, result);
	        return job.Schedule();
        }
    }
}