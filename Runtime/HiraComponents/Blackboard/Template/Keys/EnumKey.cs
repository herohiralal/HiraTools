using System;
using UnityEngine;

#if UNITY_EDITOR
using System.Reflection;
using Type = System.Type;
using UnityEditor;
using static UnityEngine.DynamicEnumValue;
#endif

namespace HiraEngine.Components.Blackboard
{
	public class EnumKey : HiraBlackboardKey
	{
		[SerializeField] public DynamicEnumValue defaultValue = null;

		public override byte SizeInBytes =>
			defaultValue.EnumUnderlyingType switch
			{
				DynamicEnumValue.Type.Byte => 1,
				DynamicEnumValue.Type.SignedByte => 1,
				DynamicEnumValue.Type.UnsignedShort => 2,
				DynamicEnumValue.Type.Short => 2,
				DynamicEnumValue.Type.UnsignedInt => 4,
				DynamicEnumValue.Type.Int => 4,
				DynamicEnumValue.Type.UnsignedLong => 8,
				DynamicEnumValue.Type.Long => 8,
				DynamicEnumValue.Type.Invalid => throw new ArgumentOutOfRangeException(),
				_ => throw new ArgumentOutOfRangeException()
			};

		public override unsafe void SetDefault(void* value)
		{
			switch (defaultValue.EnumUnderlyingType)
			{
				case DynamicEnumValue.Type.Byte:
					*(byte*) value = defaultValue.byteValue;
					break;
				case DynamicEnumValue.Type.SignedByte:
					*(sbyte*) value = defaultValue.sByteValue;
					break;
				case DynamicEnumValue.Type.UnsignedShort:
					*(ushort*) value = defaultValue.uShortValue;
					break;
				case DynamicEnumValue.Type.Short:
					*(short*) value = defaultValue.shortValue;
					break;
				case DynamicEnumValue.Type.UnsignedInt:
					*(uint*) value = defaultValue.uIntValue;
					break;
				case DynamicEnumValue.Type.Int:
					*(int*) value = defaultValue.intValue;
					break;
				case DynamicEnumValue.Type.UnsignedLong:
					*(ulong*) value = defaultValue.uLongValue;
					break;
				case DynamicEnumValue.Type.Long:
					*(long*) value = defaultValue.longValue;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

#if UNITY_EDITOR
		public override unsafe void DrawEditor(void* data, IBlackboardComponent blackboard)
		{
			switch (defaultValue.EnumUnderlyingType)
			{
				case DynamicEnumValue.Type.Byte:
					DynamicPopup(*(byte*) data, blackboard, defaultValue.EnumType);
					break;
				case DynamicEnumValue.Type.SignedByte:
					DynamicPopup(*(sbyte*) data, blackboard, defaultValue.EnumType);
					break;
				case DynamicEnumValue.Type.UnsignedShort:
					DynamicPopup(*(ushort*) data, blackboard, defaultValue.EnumType);
					break;
				case DynamicEnumValue.Type.Short:
					DynamicPopup(*(short*) data, blackboard, defaultValue.EnumType);
					break;
				case DynamicEnumValue.Type.UnsignedInt:
					DynamicPopup(*(uint*) data, blackboard, defaultValue.EnumType);
					break;
				case DynamicEnumValue.Type.Int:
					DynamicPopup(*(int*) data, blackboard, defaultValue.EnumType);
					break;
				case DynamicEnumValue.Type.UnsignedLong:
					DynamicPopup(*(ulong*) data, blackboard, defaultValue.EnumType);
					break;
				case DynamicEnumValue.Type.Long:
					DynamicPopup(*(long*) data, blackboard, defaultValue.EnumType);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void DynamicPopup<T>(T value, IBlackboardComponent blackboard, Type enumType) where T : unmanaged
		{
			var enumCast = RuntimeCastToEnum(value, enumType);
			var output =
				enumType.GetCustomAttribute<FlagsAttribute>() == null
					? EditorGUILayout.EnumPopup(name, enumCast)
					: EditorGUILayout.EnumFlagsField(name, enumCast);

			var outputCast = RuntimeCastToUnderlyingType<T>(output);
			if (!Equals(outputCast, value)) blackboard.SetValue(Index, outputCast);
		}
#endif
	}
}