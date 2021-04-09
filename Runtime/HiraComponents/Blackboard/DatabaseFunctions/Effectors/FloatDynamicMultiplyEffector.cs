using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardEffector]
	public unsafe class FloatDynamicMultiplyEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _multiplyFloatEffectorFunction;
		private static FunctionPointer<EffectorDelegate> _multiplyIntegerEffectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_multiplyFloatEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(MultiplyFloatEffector);
			_multiplyIntegerEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(MultiplyIntegerEffector);
		}

		[HiraCollectionDropdown(typeof(FloatKey))]
		[SerializeField] public HiraBlackboardKey key = null;
		[HiraCollectionDropdown(typeof(INumericalBlackboardKey))]
		[SerializeField] public HiraBlackboardKey toMultiply = null;

		public byte MemorySize => 2 * sizeof(ushort);

		public void AppendMemory(byte* stream)
		{
			var actualStream = (ushort*) stream;
			actualStream[0] = key.Index;
			actualStream[1] = toMultiply.Index;
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void MultiplyFloatEffector(byte* blackboard, byte* memory)
		{
			var actualStream = (ushort*) memory;
			*(float*) (blackboard + actualStream[0]) *= *(float*) (blackboard + actualStream[1]);
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void MultiplyIntegerEffector(byte* blackboard, byte* memory)
		{
			var actualStream = (ushort*) memory;
			*(float*) (blackboard + actualStream[0]) *= *(int*) (blackboard + actualStream[1]);
		}

		public FunctionPointer<EffectorDelegate> Function =>
			toMultiply switch
			{
				FloatKey _ => _multiplyFloatEffectorFunction,
				IntegerKey _ => _multiplyIntegerEffectorFunction,
				_ => throw new ArgumentOutOfRangeException(nameof(toMultiply))
			};

        public void ApplyTo(IBlackboardComponent blackboard)
        {
            var value = toMultiply switch
            {
                FloatKey _ => blackboard.GetValue<float>(toMultiply.Index),
                IntegerKey _ => blackboard.GetValue<int>(toMultiply.Index),
                _ => throw new ArgumentOutOfRangeException()
            };

            blackboard.SetValue<float>(key.Index, blackboard.GetValue<float>(key.Index) * value);
        }

		public override string ToString() => key == null || toMultiply == null ? "INVALID EFFECT" : $"{key.name} multiplied by {toMultiply.name}";
		private void OnValidate() => name = ToString();
	}
}