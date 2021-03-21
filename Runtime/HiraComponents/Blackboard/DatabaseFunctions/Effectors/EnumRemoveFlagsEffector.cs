using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile]
	public unsafe class EnumRemoveFlagsEffector : ScriptableObject, IBlackboardEffector
	{
		private static FunctionPointer<EffectorDelegate> _1ByteEffectorFunction;
		private static FunctionPointer<EffectorDelegate> _2ByteEffectorFunction;
		private static FunctionPointer<EffectorDelegate> _4ByteEffectorFunction;
		private static FunctionPointer<EffectorDelegate> _8ByteEffectorFunction;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CompileFunctions()
		{
			_1ByteEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector1Byte);
			_2ByteEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector2Byte);
			_4ByteEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector4Byte);
			_8ByteEffectorFunction = BurstCompiler.CompileFunctionPointer<EffectorDelegate>(Effector8Byte);
		}

		[HiraCollectionDropdown(typeof(EnumKey))]
		[SerializeField] public EnumKey key = null;
		[SerializeField] public DynamicEnumValue value = null;

		private void OnValidate()
		{
			if (key != null)
				value.type = key.defaultValue.type;

			name = ToString();
		}

		public byte MemorySize => (byte) (sizeof(ushort) + value.MemorySize);

		public void AppendMemory(byte* stream)
		{
			*(ushort*) stream = key.Index;
			value.CopyValueTo(stream + sizeof(ushort));
		}

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector1Byte(byte* blackboard, byte* memory) =>
			*(blackboard + *(ushort*) memory) &= (byte) ~(*(memory + sizeof(ushort)));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector2Byte(byte* blackboard, byte* memory) =>
			*(ushort*) (blackboard + *(ushort*) memory) &= (ushort) ~(*(ushort*) (memory + sizeof(ushort)));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector4Byte(byte* blackboard, byte* memory) =>
			*(uint*) (blackboard + *(ushort*) memory) &= ~(*(uint*) (memory + sizeof(ushort)));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector8Byte(byte* blackboard, byte* memory) =>
			*(ulong*) (blackboard + *(ushort*) memory) &= ~(*(ulong*) (memory + sizeof(ushort)));

		public FunctionPointer<EffectorDelegate> Function =>
			value.EnumUnderlyingType switch
			{
				DynamicEnumValue.Type.Byte => _1ByteEffectorFunction,
				DynamicEnumValue.Type.SignedByte => _1ByteEffectorFunction,
				DynamicEnumValue.Type.UnsignedShort => _2ByteEffectorFunction,
				DynamicEnumValue.Type.Short => _2ByteEffectorFunction,
				DynamicEnumValue.Type.UnsignedInt => _4ByteEffectorFunction,
				DynamicEnumValue.Type.Int => _4ByteEffectorFunction,
				DynamicEnumValue.Type.UnsignedLong => _8ByteEffectorFunction,
				DynamicEnumValue.Type.Long => _8ByteEffectorFunction,
				DynamicEnumValue.Type.Invalid => throw new ArgumentOutOfRangeException(),
				_ => throw new ArgumentOutOfRangeException()
			};

		public override string ToString() => key == null || !value.IsValid ? "INVALID EFFECT" : $"Remove ({value}) flags from {key.name}";
	}
}