using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Assertions;

namespace HiraEngine.Components.Blackboard
{
    [BurstCompile]
    public static unsafe class MemoryBatchHelpers
    {
        public static byte GetBlockSize<T>(this IBlackboardDatabaseFunction<T> databaseFunction) where T : Delegate
        {
            // Structure - size (1 byte), function-pointer (1 intptr), decorator state (variable)
            byte size = 0;

            size += sizeof(byte); // block size (size of fn-ptr + memory size)

            size += (byte) sizeof(FunctionPointer<T>);

            size += databaseFunction.MemorySize;

            return size;
        }

        public static byte AppendEntireBlock<T>(this IBlackboardDatabaseFunction<T> databaseFunction, byte* stream) where T : Delegate
        {
            // Structure - size (1 byte), function-pointer (1 intptr), decorator state (variable)
            var size = (byte) (databaseFunction.MemorySize + sizeof(FunctionPointer<T>) + sizeof(byte)); // self size & size of function pointer
            *stream = size;
            *(FunctionPointer<T>*) (stream + sizeof(byte)) = databaseFunction.Function;
            databaseFunction.AppendMemory(stream + sizeof(byte) + sizeof(FunctionPointer<T>));
            return size;
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertSizeEquality<T>(this IBlackboardDatabaseFunction<T> databaseFunction, byte compareTo) where T : Delegate =>
            Assert.AreEqual(databaseFunction.GetBlockSize(), compareTo);

        public static ushort GetBlockSize<T>(this IEnumerable<IBlackboardDatabaseFunction<T>> databaseFunctions) where T : Delegate
        {
            // Structure - total block-size (1 ushort), function-count (1 byte), { database function 1 block, database function 2 block, database function 3 block... }
            ushort size = 0;

            size += sizeof(ushort); // size of whole memory block (size of each individual memory block added together + number of elements)

            size += sizeof(byte); // number of elements in the array

            foreach (var function in databaseFunctions)
            {
                size += function.GetBlockSize();
            }

            return size;
        }

        public static ushort AppendEntireBlock<T>(this IEnumerable<IBlackboardDatabaseFunction<T>> databaseFunctions, byte* stream) where T : Delegate
        {
            // Structure - total block-size (1 ushort), function block count (1 byte), { database function 1 block, database function 2 block, database function 3 block... }

            ushort size = sizeof(byte) + sizeof(ushort); // total header size (total block-size, function block count)
            byte count = 0;

            var actualDataAddress = stream + size;
            foreach (var function in databaseFunctions)
            {
                var currentFunctionBlockSize = function.AppendEntireBlock(actualDataAddress);
                AssertSizeEquality(function, currentFunctionBlockSize);

                actualDataAddress += currentFunctionBlockSize;
                size += currentFunctionBlockSize;
                count++;
            }

            // header
            *(ushort*) stream = size; // total block size
            *(stream + sizeof(ushort)) = count; // function block count

            return size;
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertSizeEquality<T>(this IEnumerable<IBlackboardDatabaseFunction<T>> databaseFunctions, ushort compareTo) where T : Delegate =>
            Assert.AreEqual(databaseFunctions.GetBlockSize(), compareTo);

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExecuteDecoratorBlock(byte* blackboard, byte* address)
        {
            address += sizeof(byte); // ignore the size header

            return UnsafeUtility.As<IntPtr, FunctionPointer<DecoratorDelegate>>(ref *(IntPtr*) address)
                .Invoke(blackboard, address + sizeof(FunctionPointer<DecoratorDelegate>));
        }

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExecuteDecoratorsBlock(byte* blackboard, byte* address)
        {
            address += sizeof(ushort); // ignore the size header

            var count = *(address++);
            for (var i = 0; i < count; i++)
            {
                if (!ExecuteDecoratorBlock(blackboard, address)) return false;
                address += *address; // the size header
            }

            return true;
        }

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ExecuteScoreCalculatorBlock(byte* blackboard, byte* address)
        {
            address += sizeof(byte); // ignore the size header
            var scoreAddress = (float*) (address + sizeof(FunctionPointer<DecoratorDelegate>));
            var memory = (byte*) (scoreAddress + 1);

            var result = UnsafeUtility.As<IntPtr, FunctionPointer<DecoratorDelegate>>(ref *(IntPtr*) address)
                .Invoke(blackboard, memory);

            return result ? *scoreAddress : 0;
        }

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ExecuteScoreCalculatorsBlock(byte* blackboard, byte* address)
        {
            address += sizeof(ushort); // ignore the size header

            var score = 0f;

            var count = *(address++);
            for (var i = 0; i < count; i++)
            {
                score += ExecuteScoreCalculatorBlock(blackboard, address);
                address += *address;
            }

            return score;
        }

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExecuteEffectorBlock(byte* blackboard, byte* address)
        {
            address += sizeof(byte); // ignore the size header

            UnsafeUtility.As<IntPtr, FunctionPointer<EffectorDelegate>>(ref *(IntPtr*) address)
                .Invoke(blackboard, address + sizeof(FunctionPointer<DecoratorDelegate>));
        }

        [BurstCompile, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExecuteEffectorsBlock(byte* blackboard, byte* address)
        {
            address += sizeof(ushort); // ignore the size header

            var count = *(address++);
            for (var i = 0; i < count; i++)
            {
                ExecuteEffectorBlock(blackboard, address);
                address += *address;
            }
        }
    }
}