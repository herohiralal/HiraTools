using System.Collections.Generic;
using System.Diagnostics;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Burst;
using UnityEngine.Assertions;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
    [BurstCompile]
    public static unsafe class LayerMemoryBatchHelpers
    {
        public static ushort GetLayerMemorySize(
            IEnumerable<IBlackboardDecorator[]> targets,
            IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions)
        {
            ushort size = 0;
            size += targets.GetGoalTargetsBlockSize();
            size += actions.GetActionsBlockSize();
            return size;
        }

        public static ushort AppendEntireLayerBlock(
            IBlackboardDecorator[][] targets,
            (IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])[] actions,
            byte* stream)
        {
            ushort size = 0;

            var targetsBlockAllocatedSize = targets.AppendGoalTargetsBlock(stream);
            targets.AssertSizeEquality(targetsBlockAllocatedSize);
            stream += targetsBlockAllocatedSize;
            size += targetsBlockAllocatedSize;

            var actionsBlockAllocatedSize = actions.AppendActionsBlock(stream);
            actions.AssertSizeEquality(actionsBlockAllocatedSize);
            size += actionsBlockAllocatedSize;

            return size;
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertSizeEquality(
            IEnumerable<IBlackboardDecorator[]> targets,
            IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions,
            ushort compareTo) => Assert.AreEqual(GetLayerMemorySize(targets, actions), compareTo);

        [BurstCompile]
        public static byte* GetLayerFromDomainData(byte* address, byte index)
        {
            address += *(ushort*) address; // skip over the insistence calculators for a goal

            for (var i = 0; i < index; i++)
            {
                address += *(ushort*) address;
                address += *(ushort*) address;
            }

            return address;
        }
    }
}