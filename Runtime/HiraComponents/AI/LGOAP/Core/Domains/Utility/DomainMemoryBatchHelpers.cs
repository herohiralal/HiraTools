using System.Collections.Generic;
using System.Diagnostics;
using HiraEngine.Components.Blackboard;
using HiraEngine.Components.Blackboard.Internal;
using UnityEngine.Assertions;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
    public static unsafe class DomainMemoryBatchHelpers
    {
        public static ushort GetInsistenceCalculatorsBlockSize(this IEnumerable<IBlackboardScoreCalculator[]> insistenceCalculators)
        {
            ushort size = 0;
            size += sizeof(ushort); // size of the whole memory block

            size += sizeof(byte); // number of insistence calculators
            
            foreach (var insistenceCalculator in insistenceCalculators)
            {
                size += insistenceCalculator.GetBlockSize();
            }

            return size;
        }

        public static ushort AppendInsistenceCalculatorsBlock(this IEnumerable<IBlackboardScoreCalculator[]> insistenceCalculators, byte* stream)
        {
            ushort size = sizeof(byte) + sizeof(ushort); // total header size (total block-size, number of insistence calculators)
            byte count = 0;

            var actualDataAddress = stream + size;
            foreach (var calculator in insistenceCalculators)
            {
                var currentInsistenceCalculatorSize = calculator.AppendEntireBlock(actualDataAddress);
                calculator.AssertSizeEquality(currentInsistenceCalculatorSize);

                actualDataAddress += currentInsistenceCalculatorSize;
                size += currentInsistenceCalculatorSize;
                count++;
            }

            // header
            *(ushort*) stream = size; // total block size
            *(stream + sizeof(ushort)) = count; // function block count

            return size;
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertSizeEquality(this IEnumerable<IBlackboardScoreCalculator[]> insistenceCalculators, ushort compareTo) =>
            Assert.AreEqual(insistenceCalculators.GetInsistenceCalculatorsBlockSize(), compareTo);

        public static ushort GetActionsBlockSize(this IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions)
        {
            ushort size = 0;
            size += sizeof(ushort); // size of the whole memory block

            size += sizeof(byte); // number of actions
            
            foreach (var (preconditions, costCalculators, effects) in actions)
            {
                size += PrimitiveMemoryBatchHelpers.GetActionBlockSize(preconditions, costCalculators, effects);
            }

            return size;
        }

        public static ushort AppendActionsBlock(this IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions, byte* stream)
        {
            ushort size = sizeof(byte) + sizeof(ushort); // total header size (total block-size, number of actions)
            byte count = 0;

            var actualDataAddress = stream + size;
            foreach (var (preconditions, costCalculators, effects) in actions)
            {
                var currentActionBlockSize = PrimitiveMemoryBatchHelpers.AppendEntireActionBlock(preconditions, costCalculators, effects, actualDataAddress);
                PrimitiveMemoryBatchHelpers.AssertSizeEquality(preconditions, costCalculators, effects, currentActionBlockSize);

                actualDataAddress += currentActionBlockSize;
                size += currentActionBlockSize;
                count++;
            }

            // header
            *(ushort*) stream = size; // total block size
            *(stream + sizeof(ushort)) = count; // function block count

            return size;
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertSizeEquality(this IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions, ushort compareTo) =>
            Assert.AreEqual(actions.GetActionsBlockSize(), compareTo);

        public static ushort GetGoalTargetsBlockSize(this IEnumerable<IBlackboardDecorator[]> targets)
        {
            ushort size = 0;
            size += sizeof(ushort); // size of the whole memory block

            size += sizeof(byte); // number of targets
            
            foreach (var target in targets)
            {
                size += target.GetBlockSize();
            }

            return size;
        }

        public static ushort AppendGoalTargetsBlock(this IEnumerable<IBlackboardDecorator[]> targets, byte* stream)
        {
            ushort size = sizeof(byte) + sizeof(ushort); // total header size (total block-size, number of targets)
            byte count = 0;

            var actualDataAddress = stream + size;
            foreach (var target in targets)
            {
                var currentTargetSize = target.AppendEntireBlock(actualDataAddress);
                target.AssertSizeEquality(currentTargetSize);

                actualDataAddress += currentTargetSize;
                size += currentTargetSize;
                count++;
            }

            // header
            *(ushort*) stream = size; // total block size
            *(stream + sizeof(ushort)) = count; // function block count

            return size;
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertSizeEquality(this IEnumerable<IBlackboardDecorator[]> targets, ushort compareTo) =>
            Assert.AreEqual(targets.GetGoalTargetsBlockSize(), compareTo);
    }
}