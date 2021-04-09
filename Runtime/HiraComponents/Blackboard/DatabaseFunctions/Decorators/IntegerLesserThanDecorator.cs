using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardDecorator]
	public unsafe class IntegerLesserThanDecorator : ScriptableObject, IBlackboardDecorator
	{
		private static FunctionPointer<DecoratorDelegate> _equalsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _invertedDecoratorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_equalsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(Decorator);
			_invertedDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(InvertedDecorator);
		}

		[HiraCollectionDropdown(typeof(IntegerKey))]
		[SerializeField] private HiraBlackboardKey key = null;
		[SerializeField] private int value = 0;
		[SerializeField] private bool invert = false;

		public virtual byte MemorySize => sizeof(ushort) + sizeof(int);
		public virtual void AppendMemory(byte* stream) => (*(ushort*) stream, *(int*) (stream + sizeof(ushort))) = (key.Index, value);

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool Decorator(byte* blackboard, byte* memory) => *(int*)(blackboard + *(ushort*) memory) < *(int*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool InvertedDecorator(byte* blackboard, byte* memory) => *(int*)(blackboard + *(ushort*) memory) >= *(int*) (memory + sizeof(ushort));

		public FunctionPointer<DecoratorDelegate> Function =>
			invert
				? _invertedDecoratorFunction
				: _equalsDecoratorFunction;

        public bool IsValidOn(IBlackboardComponent blackboard) =>
            invert != (blackboard.GetValue<int>(key.Index) < value);

		public override string ToString() => key == null ? "INVALID CONDITION" : $"{key.name} {(invert ? "not" : "")} lesser than {value}";
		private void OnValidate() => name = ToString();
	}
}