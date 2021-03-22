using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HiraEngine.Components.Blackboard;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Burst;
using UnityEngine.Assertions;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
    [BurstCompile]
    public static unsafe class PrimitiveMemoryBatchHelpers
    {
        public static ushort GetActionBlockSize(
            IEnumerable<IBlackboardDecorator> preconditions,
            IEnumerable<IBlackboardScoreCalculator> costCalculators,
            IEnumerable<IBlackboardEffector> effects)
        {
                ushort size = 0;

                size += sizeof(ushort); // size of the whole block

                // preconditions
                size += preconditions.GetBlockSize();

                // cost calculators
                size += costCalculators.GetBlockSize();

                // effects
                size += effects.GetBlockSize();

                return size;
        }

        public static ushort AppendEntireActionBlock(
            IBlackboardDecorator[] preconditions,
            IBlackboardScoreCalculator[] costCalculators,
            IBlackboardEffector[] effects,
            byte* address)
        {
            ushort size = sizeof(ushort); // total block size header

            var a = address + sizeof(ushort); // ignore size header

            var currentArrayBlockSize = preconditions.AppendEntireBlock(a);
            preconditions.AssertSizeEquality(currentArrayBlockSize);
            a += currentArrayBlockSize;
            size += currentArrayBlockSize;

            currentArrayBlockSize = costCalculators.AppendEntireBlock(a);
            costCalculators.AssertSizeEquality(currentArrayBlockSize);
            a += currentArrayBlockSize;
            size += currentArrayBlockSize;

            currentArrayBlockSize = effects.AppendEntireBlock(a);
            effects.AssertSizeEquality(currentArrayBlockSize);
            size += currentArrayBlockSize;

            *(ushort*) address = size;

            return size; // for the size value
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertSizeEquality(
            IEnumerable<IBlackboardDecorator> preconditions,
            IEnumerable<IBlackboardScoreCalculator> costCalculators,
            IEnumerable<IBlackboardEffector> effects,
            ushort compareTo) => Assert.AreEqual(GetActionBlockSize(preconditions, costCalculators, effects), compareTo);

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* GetPreconditionsBlockFromActionBlock(byte* address) => address + sizeof(ushort); // skip over size header

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* GetCostCalculatorsBlockFromActionBlock(byte* address)
        {
            var preconditionsBlock = GetPreconditionsBlockFromActionBlock(address);
            return preconditionsBlock + *(ushort*) preconditionsBlock;
        }

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* GetEffectsBlockFromActionBlock(byte* address)
        {
            var costCalculatorsBlock = GetCostCalculatorsBlockFromActionBlock(address);
            return costCalculatorsBlock + *(ushort*) costCalculatorsBlock;
        }
    }
}