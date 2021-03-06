﻿using System;
using Unity.Burst;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
	[Serializable, BurstCompile, HiraBlackboardEffector]
	public unsafe class EnumAddFlagsEffector : ScriptableObject, IBlackboardEffector
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
			*(blackboard + *(ushort*) memory) |= *(memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector2Byte(byte* blackboard, byte* memory) =>
			*(ushort*) (blackboard + *(ushort*) memory) |= *(ushort*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector4Byte(byte* blackboard, byte* memory) =>
			*(uint*) (blackboard + *(ushort*) memory) |= *(uint*) (memory + sizeof(ushort));

		[BurstCompile, AOT.MonoPInvokeCallback(typeof(EffectorDelegate))]
		private static void Effector8Byte(byte* blackboard, byte* memory) =>
			*(ulong*) (blackboard + *(ushort*) memory) |= *(ulong*) (memory + sizeof(ushort));

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

        public void ApplyTo(IBlackboardComponent blackboard)
        {
            switch (value.EnumUnderlyingType)
            {
                case DynamicEnumValue.Type.Byte:
                    blackboard.SetValue<byte>(key.Index, (byte) (blackboard.GetValue<byte>(key.Index) | value.byteValue));
                    break;
                case DynamicEnumValue.Type.SignedByte:
                    blackboard.SetValue<sbyte>(key.Index, (sbyte) (blackboard.GetValue<sbyte>(key.Index) | value.sByteValue));
                    break;
                case DynamicEnumValue.Type.UnsignedShort:
                    blackboard.SetValue<ushort>(key.Index, (ushort) (blackboard.GetValue<ushort>(key.Index) | value.uShortValue));
                    break;
                case DynamicEnumValue.Type.Short:
                    blackboard.SetValue<short>(key.Index, (short) (blackboard.GetValue<short>(key.Index) | value.shortValue));
                    break;
                case DynamicEnumValue.Type.UnsignedInt:
                    blackboard.SetValue<uint>(key.Index, (uint) (blackboard.GetValue<uint>(key.Index) | value.uIntValue));
                    break;
                case DynamicEnumValue.Type.Int:
                    blackboard.SetValue<int>(key.Index, (int) (blackboard.GetValue<int>(key.Index) | value.intValue));
                    break;
                case DynamicEnumValue.Type.UnsignedLong:
                    blackboard.SetValue<ulong>(key.Index, (ulong) (blackboard.GetValue<ulong>(key.Index) | value.uLongValue));
                    break;
                case DynamicEnumValue.Type.Long:
                    blackboard.SetValue<long>(key.Index, (long) (blackboard.GetValue<long>(key.Index) | value.longValue));
                    break;
                case DynamicEnumValue.Type.Invalid:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

		public override string ToString() => key == null || !value.IsValid ? "INVALID EFFECT" : $"Add ({value}) flags to {key.name}";
	}
}