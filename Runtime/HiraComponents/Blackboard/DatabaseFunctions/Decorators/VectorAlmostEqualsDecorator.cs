using System;
using Unity.Burst;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardDecorator]
	public unsafe class VectorAlmostEqualsDecorator : ScriptableObject, IBlackboardDecorator
	{
		private static FunctionPointer<DecoratorDelegate> _equalsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _notEqualsDecoratorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_equalsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsDecorator);
			_notEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(NotEqualsDecorator);
		}

		[HiraCollectionDropdown(typeof(VectorKey))]
		[SerializeField] private HiraBlackboardKey key = null;
		[SerializeField] private Vector3 value = Vector3.zero;
		[SerializeField] private Vector3 tolerance = Vector3.zero;
		[SerializeField] private bool invert = false;

		public byte MemorySize => sizeof(ushort) + (6 * sizeof(float));

		public void AppendMemory(byte* stream)
		{
			*(ushort*) stream = key.Index;
			var valueAddress = (float*) (stream + sizeof(ushort));
			valueAddress[0] = value.x;
			valueAddress[1] = value.y;
			valueAddress[2] = value.z;
			valueAddress[3] = tolerance.x;
			valueAddress[4] = tolerance.y;
			valueAddress[5] = tolerance.z;
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsDecorator(byte* blackboard, byte* memory)
		{
			var currentValue = (float*) (blackboard + *(ushort*) memory);
			var value = (float*) (memory + sizeof(ushort));
			return (Unity.Mathematics.math.abs(currentValue[0] - value[0]) <= value[3])
			       && (Unity.Mathematics.math.abs(currentValue[1] - value[1]) <= value[4])
			       && (Unity.Mathematics.math.abs(currentValue[2] - value[2]) <= value[5]);
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool NotEqualsDecorator(byte* blackboard, byte* memory)
		{
			var currentValue = (float*) (blackboard + *(ushort*) memory);
			var value = (float*) (memory + sizeof(ushort));
			return (Unity.Mathematics.math.abs(currentValue[0] - value[0]) > value[3])
			       || (Unity.Mathematics.math.abs(currentValue[1] - value[1]) > value[4])
			       || (Unity.Mathematics.math.abs(currentValue[2] - value[2]) > value[5]);
		}

		public FunctionPointer<DecoratorDelegate> Function =>
			invert
				? _notEqualsDecoratorFunction
				: _equalsDecoratorFunction;

		public override string ToString() => key == null ? "INVALID CONDITION" : $"{key.name} is {(invert ? "not" : "")} between {value - tolerance} and {value + tolerance}";
		private void OnValidate() => name = ToString();
	}
}