using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardDecorator]
	public unsafe class BooleanEqualsDecorator : ScriptableObject, IBlackboardDecorator
	{
		private static FunctionPointer<DecoratorDelegate> _equalsTrueDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _equalsFalseDecoratorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_equalsTrueDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsTrueDecorator);
			_equalsFalseDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsFalseDecorator);
		}

		[HiraCollectionDropdown(typeof(BooleanKey))]
		[SerializeField] private HiraBlackboardKey key = null;
		[SerializeField] private bool value = true;

		public byte MemorySize => sizeof(ushort);
		public void AppendMemory(byte* stream) => *(ushort*) stream = key.Index;

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsTrueDecorator(byte* blackboard, byte* memory) => blackboard[*(ushort*) memory] != 0;

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsFalseDecorator(byte* blackboard, byte* memory) => blackboard[*(ushort*) memory] == 0;

		public FunctionPointer<DecoratorDelegate> Function =>
			value
				? _equalsTrueDecoratorFunction
				: _equalsFalseDecoratorFunction;

		public override string ToString() => key == null ? "INVALID CONDITION" : $"{key.name} equals {value}";
		private void OnValidate() => name = ToString();
	}
}