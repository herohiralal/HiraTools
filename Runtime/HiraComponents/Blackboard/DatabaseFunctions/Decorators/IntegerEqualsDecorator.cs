using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardDecorator]
	public unsafe class IntegerEqualsDecorator : ScriptableObject, IBlackboardDecorator
	{
		private static FunctionPointer<DecoratorDelegate> _equalsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _notEqualsDecoratorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_equalsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsDecorator);
			_notEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(NotEqualsDecorator);
		}

		[HiraCollectionDropdown(typeof(IntegerKey))]
		[SerializeField] private HiraBlackboardKey key = null;
		[SerializeField] private int value = 0;
		[SerializeField] private bool invert = false;

		public virtual byte MemorySize => sizeof(ushort) + sizeof(int);
		public virtual void AppendMemory(byte* stream) => (*(ushort*) stream, *(int*) (stream + sizeof(ushort))) = (key.Index, value);

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsDecorator(byte* blackboard, byte* memory) => *(int*)(blackboard + *(ushort*) memory) == *(int*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool NotEqualsDecorator(byte* blackboard, byte* memory) => *(int*)(blackboard + *(ushort*) memory) != *(int*) (memory + sizeof(ushort));

		public FunctionPointer<DecoratorDelegate> Function =>
			invert
				? _notEqualsDecoratorFunction
				: _equalsDecoratorFunction;

		public override string ToString() => key == null ? "INVALID CONDITION" : $"{key.name} {(invert ? "not" : "")} equals {value}";
		private void OnValidate() => name = ToString();
	}
}