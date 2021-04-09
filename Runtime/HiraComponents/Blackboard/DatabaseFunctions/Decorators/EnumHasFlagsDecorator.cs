using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardDecorator]
	public unsafe class EnumHasFlagsDecorator : ScriptableObject, IBlackboardDecorator
	{
		private static FunctionPointer<DecoratorDelegate> _1ByteDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _2ByteDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _4ByteDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _8ByteDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _1ByteInvertedDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _2ByteInvertedDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _4ByteInvertedDecoratorFunction;
		private static FunctionPointer<DecoratorDelegate> _8ByteInvertedDecoratorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_1ByteDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(Decorator1Byte);
			_2ByteDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(Decorator2Byte);
			_4ByteDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(Decorator4Byte);
			_8ByteDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(Decorator8Byte);
			_1ByteInvertedDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(InvertedDecorator1Byte);
			_2ByteInvertedDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(InvertedDecorator2Byte);
			_4ByteInvertedDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(InvertedDecorator4Byte);
			_8ByteInvertedDecoratorFunction = BurstCompiler.CompileFunctionPointer<DecoratorDelegate>(InvertedDecorator8Byte);
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
		private static bool Decorator1Byte(byte* blackboard, byte* memory) =>
			(*(blackboard + *(ushort*) memory) & *(memory + sizeof(ushort))) == *(memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool Decorator2Byte(byte* blackboard, byte* memory) =>
			(*(ushort*) (blackboard + *(ushort*) memory) & *(ushort*) (memory + sizeof(ushort))) == *(ushort*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool Decorator4Byte(byte* blackboard, byte* memory) =>
			(*(uint*) (blackboard + *(ushort*) memory) & *(uint*) (memory + sizeof(ushort))) == *(uint*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool Decorator8Byte(byte* blackboard, byte* memory) =>
			(*(ulong*) (blackboard + *(ushort*) memory) & *(ulong*) (memory + sizeof(ushort))) == *(ulong*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool InvertedDecorator1Byte(byte* blackboard, byte* memory) =>
			(*(blackboard + *(ushort*) memory) & *(memory + sizeof(ushort))) != *(memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool InvertedDecorator2Byte(byte* blackboard, byte* memory) =>
			(*(ushort*) (blackboard + *(ushort*) memory) & *(ushort*) (memory + sizeof(ushort))) != *(ushort*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool InvertedDecorator4Byte(byte* blackboard, byte* memory) =>
			(*(uint*) (blackboard + *(ushort*) memory) & *(uint*) (memory + sizeof(ushort))) != *(uint*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(DecoratorDelegate))]
		private static bool InvertedDecorator8Byte(byte* blackboard, byte* memory) =>
			(*(ulong*) (blackboard + *(ushort*) memory) & *(ulong*) (memory + sizeof(ushort))) != *(ulong*) (memory + sizeof(ushort));

		public FunctionPointer<DecoratorDelegate> Function =>
			invert switch
			{
				false => value.EnumUnderlyingType switch
				{
					DynamicEnumValue.Type.Byte => _1ByteDecoratorFunction,
					DynamicEnumValue.Type.SignedByte => _1ByteDecoratorFunction,
					DynamicEnumValue.Type.UnsignedShort => _2ByteDecoratorFunction,
					DynamicEnumValue.Type.Short => _2ByteDecoratorFunction,
					DynamicEnumValue.Type.UnsignedInt => _4ByteDecoratorFunction,
					DynamicEnumValue.Type.Int => _4ByteDecoratorFunction,
					DynamicEnumValue.Type.UnsignedLong => _8ByteDecoratorFunction,
					DynamicEnumValue.Type.Long => _8ByteDecoratorFunction,
					DynamicEnumValue.Type.Invalid => throw new ArgumentOutOfRangeException(),
					_ => throw new ArgumentOutOfRangeException()
				},
				true => value.EnumUnderlyingType switch
				{
					DynamicEnumValue.Type.Byte => _1ByteInvertedDecoratorFunction,
					DynamicEnumValue.Type.SignedByte => _1ByteInvertedDecoratorFunction,
					DynamicEnumValue.Type.UnsignedShort => _2ByteInvertedDecoratorFunction,
					DynamicEnumValue.Type.Short => _2ByteInvertedDecoratorFunction,
					DynamicEnumValue.Type.UnsignedInt => _4ByteInvertedDecoratorFunction,
					DynamicEnumValue.Type.Int => _4ByteInvertedDecoratorFunction,
					DynamicEnumValue.Type.UnsignedLong => _8ByteInvertedDecoratorFunction,
					DynamicEnumValue.Type.Long => _8ByteInvertedDecoratorFunction,
					DynamicEnumValue.Type.Invalid => throw new ArgumentOutOfRangeException(),
					_ => throw new ArgumentOutOfRangeException()
				}
			};

        public bool IsValidOn(IBlackboardComponent blackboard) =>
            value.EnumUnderlyingType switch
            {
                DynamicEnumValue.Type.Byte => invert != ((blackboard.GetValue<byte>(key.Index) & value.byteValue) == value.byteValue),
                DynamicEnumValue.Type.SignedByte => invert != ((blackboard.GetValue<sbyte>(key.Index) & value.sByteValue) == value.sByteValue),
                DynamicEnumValue.Type.UnsignedShort => invert != ((blackboard.GetValue<ushort>(key.Index) & value.uShortValue) == value.uShortValue),
                DynamicEnumValue.Type.Short => invert != ((blackboard.GetValue<short>(key.Index) & value.shortValue) == value.shortValue),
                DynamicEnumValue.Type.UnsignedInt => invert != ((blackboard.GetValue<uint>(key.Index) & value.uIntValue) == value.uIntValue),
                DynamicEnumValue.Type.Int => invert != ((blackboard.GetValue<int>(key.Index) & value.intValue) == value.intValue),
                DynamicEnumValue.Type.UnsignedLong => invert != ((blackboard.GetValue<ulong>(key.Index) & value.uLongValue) == value.uLongValue),
                DynamicEnumValue.Type.Long => invert != ((blackboard.GetValue<long>(key.Index) & value.longValue) == value.longValue),
                DynamicEnumValue.Type.Invalid => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };

		public override string ToString() => key == null || !value.IsValid ? "INVALID CONDITION" : $"{key.name} {(invert ? "lacks" : "has")} ({value}) flags";
	}
}