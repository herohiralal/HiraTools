using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
    [Serializable, BurstCompile, HiraBlackboardDecorator]
    public unsafe class AlwaysSucceedDecorator : ScriptableObject, IBlackboardDecorator
    {
        private static FunctionPointer<DecoratorDelegate> _function;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void CompileFunctions()
        {
            _function = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(FunctionImpl);
        }

        public virtual byte MemorySize => 0;

        public virtual void AppendMemory(byte* memory)
        {
        }

        [BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
        private static bool FunctionImpl(byte* blackboard, byte* memory) => true;

        public FunctionPointer<DecoratorDelegate> Function => _function;

        public override string ToString() => "Always Succeed";
        private void OnValidate() => name = ToString();
    }
}