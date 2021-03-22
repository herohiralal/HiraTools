using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardDecorator]
	public unsafe class EnumEqualsDecorator : ScriptableObject, IBlackboardDecorator
	{
		private static FunctionPointer<DecoratorDelegate> _1ByteEqualsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _2ByteEqualsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _4ByteEqualsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _8ByteEqualsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _1ByteNotEqualsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _2ByteNotEqualsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _4ByteNotEqualsDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _8ByteNotEqualsDecoratorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_1ByteEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsDecorator1Byte);
			_2ByteEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsDecorator2Byte);
			_4ByteEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsDecorator4Byte);
			_8ByteEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(EqualsDecorator8Byte);
			_1ByteNotEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(NotEqualsDecorator1Byte);
			_2ByteNotEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(NotEqualsDecorator2Byte);
			_4ByteNotEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(NotEqualsDecorator4Byte);
			_8ByteNotEqualsDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(NotEqualsDecorator8Byte);
		}

		[HiraCollectionDropdown(typeof(EnumKey))]
		[SerializeField] public EnumKey key = null;
		[SerializeField] public DynamicEnumValue value = null;
		[SerializeField] public bool invert = false;

		private void OnValidate()
		{
			if (key != null)
				value.type = key.defaultValue.type;

			name = ToString();
		}

		public virtual byte MemorySize => (byte) (sizeof(ushort) + value.MemorySize);

		public virtual void AppendMemory(byte* stream)
		{
			*(ushort*) stream = key.Index;
			value.CopyValueTo(stream + sizeof(ushort));
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsDecorator1Byte(byte* blackboard, byte* memory) =>
			*(blackboard + *(ushort*) memory) == *(memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsDecorator2Byte(byte* blackboard, byte* memory) =>
			*(ushort*) (blackboard + *(ushort*) memory) == *(ushort*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsDecorator4Byte(byte* blackboard, byte* memory) =>
			*(uint*) (blackboard + *(ushort*) memory) == *(uint*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool EqualsDecorator8Byte(byte* blackboard, byte* memory) =>
			*(ulong*) (blackboard + *(ushort*) memory) == *(ulong*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool NotEqualsDecorator1Byte(byte* blackboard, byte* memory) =>
			*(blackboard + *(ushort*) memory) != *(memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool NotEqualsDecorator2Byte(byte* blackboard, byte* memory) =>
			*(ushort*) (blackboard + *(ushort*) memory) != *(ushort*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool NotEqualsDecorator4Byte(byte* blackboard, byte* memory) =>
			*(uint*) (blackboard + *(ushort*) memory) != *(uint*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool NotEqualsDecorator8Byte(byte* blackboard, byte* memory) =>
			*(ulong*) (blackboard + *(ushort*) memory) != *(ulong*) (memory + sizeof(ushort));

		public FunctionPointer<DecoratorDelegate> Function =>
			invert switch
			{
				false => value.EnumUnderlyingType switch
				{
					DynamicEnumValue.Type.Byte => _1ByteEqualsDecoratorFunction,
					DynamicEnumValue.Type.SignedByte => _1ByteEqualsDecoratorFunction,
					DynamicEnumValue.Type.UnsignedShort => _2ByteEqualsDecoratorFunction,
					DynamicEnumValue.Type.Short => _2ByteEqualsDecoratorFunction,
					DynamicEnumValue.Type.UnsignedInt => _4ByteEqualsDecoratorFunction,
					DynamicEnumValue.Type.Int => _4ByteEqualsDecoratorFunction,
					DynamicEnumValue.Type.UnsignedLong => _8ByteEqualsDecoratorFunction,
					DynamicEnumValue.Type.Long => _8ByteEqualsDecoratorFunction,
					DynamicEnumValue.Type.Invalid => throw new ArgumentOutOfRangeException(),
					_ => throw new ArgumentOutOfRangeException()
				},
				true => value.EnumUnderlyingType switch
				{
					DynamicEnumValue.Type.Byte => _1ByteNotEqualsDecoratorFunction,
					DynamicEnumValue.Type.SignedByte => _1ByteNotEqualsDecoratorFunction,
					DynamicEnumValue.Type.UnsignedShort => _2ByteNotEqualsDecoratorFunction,
					DynamicEnumValue.Type.Short => _2ByteNotEqualsDecoratorFunction,
					DynamicEnumValue.Type.UnsignedInt => _4ByteNotEqualsDecoratorFunction,
					DynamicEnumValue.Type.Int => _4ByteNotEqualsDecoratorFunction,
					DynamicEnumValue.Type.UnsignedLong => _8ByteNotEqualsDecoratorFunction,
					DynamicEnumValue.Type.Long => _8ByteNotEqualsDecoratorFunction,
					DynamicEnumValue.Type.Invalid => throw new ArgumentOutOfRangeException(),
					_ => throw new ArgumentOutOfRangeException()
				}
			};

		public override string ToString() => key == null || !value.IsValid ? "INVALID CONDITION" : $"{key.name} {(invert ? "not" : "")} equals {value}";
	}
}