using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile]
	public unsafe class FloatDynamicAddEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _addFloatEffectorFunction;
		private static FunctionPointer<EffectorDelegate> _addIntegerEffectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_addFloatEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(AddFloatEffector);
			_addIntegerEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(AddIntegerEffector);
		}

		[HiraCollectionDropdown(typeof(FloatKey))]
		[SerializeField] public HiraBlackboardKey key = null;
		[HiraCollectionDropdown(typeof(INumericalBlackboardKey))]
		[SerializeField] public HiraBlackboardKey toAdd = null;

		public byte MemorySize => 2 * sizeof(ushort);

		public void AppendMemory(byte* stream)
		{
			var actualStream = (ushort*) stream;
			actualStream[0] = key.Index;
			actualStream[1] = toAdd.Index;
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void AddFloatEffector(byte* blackboard, byte* memory)
		{
			var actualStream = (ushort*) memory;
			*(float*) (blackboard + actualStream[0]) += *(float*) (blackboard + actualStream[1]);
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void AddIntegerEffector(byte* blackboard, byte* memory)
		{
			var actualStream = (ushort*) memory;
			*(float*) (blackboard + actualStream[0]) += *(int*) (blackboard + actualStream[1]);
		}

		public FunctionPointer<EffectorDelegate> Function =>
			toAdd switch
			{
				FloatKey _ => _addFloatEffectorFunction,
				IntegerKey _ => _addIntegerEffectorFunction,
				_ => throw new ArgumentOutOfRangeException(nameof(toAdd))
			};

		public override string ToString() => key == null || toAdd == null ? "INVALID EFFECT" : $"{key.name} plus {toAdd.name}";
		private void OnValidate() => name = ToString();
	}
}