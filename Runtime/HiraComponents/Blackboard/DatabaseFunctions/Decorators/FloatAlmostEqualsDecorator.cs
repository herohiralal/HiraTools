using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardDecorator]
	public unsafe class FloatAlmostEqualsDecorator : ScriptableObject, IBlackboardDecorator
	{
		private static FunctionPointer<DecoratorDelegate> _equalsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _notEqualsDecoratorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_equalsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsDecorator);
			_notEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(NotEqualsDecorator);
		}

		[HiraCollectionDropdown(typeof(FloatKey))]
		[SerializeField] private HiraBlackboardKey key = null;
		[SerializeField] private float value = 0;
		[SerializeField] private float tolerance = 0.1f;
		[SerializeField] private bool invert = false;

		public byte MemorySize => sizeof(ushort) + (2 * sizeof(float));

		public void AppendMemory(byte* stream)
		{
			*(ushort*) stream = key.Index;
			*(float*) (stream + sizeof(ushort)) = value;
			*(float*) (stream + sizeof(ushort) + sizeof(float)) = tolerance;
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsDecorator(byte* blackboard, byte* memory)
		{
			var currentValue = *(float*) (blackboard + *(ushort*) memory);
			var value = (float*) (memory + sizeof(ushort));
			return Unity.Mathematics.math.abs(currentValue - value[0]) <= value[1];
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool NotEqualsDecorator(byte* blackboard, byte* memory)
		{
			var currentValue = *(float*) (blackboard + *(ushort*) memory);
			var value = (float*) (memory + sizeof(ushort));
			return Unity.Mathematics.math.abs(currentValue - value[0]) > value[1];
		}

		public FunctionPointer<DecoratorDelegate> Function =>
			invert
				? _notEqualsDecoratorFunction
				: _equalsDecoratorFunction;

		public override string ToString() => key == null ? "INVALID CONDITION" : $"{key.name} is {(invert ? "not" : "")} between {value - tolerance} and {value + tolerance}";
		private void OnValidate() => name = ToString();
	}
}