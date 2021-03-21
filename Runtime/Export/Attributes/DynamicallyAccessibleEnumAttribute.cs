using System;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
using System.Linq.Expressions;
using UnityEditor;
#endif

namespace UnityEngine
{
	[AttributeUsage(AttributeTargets.Enum)]
	public class DynamicallyAccessibleEnumAttribute : Attribute
	{
#if UNITY_EDITOR
		[InitializeOnLoadMethod]
#endif
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Initialize()
		{
			var types = typeof(DynamicallyAccessibleEnumAttribute).GetTypesWithThisAttribute();
			foreach (var type in types)
			{
				var identifier = type.GetCustomAttribute<DynamicallyAccessibleEnumAttribute>().Identifier;
				if (!DATABASE.ContainsKey(identifier)) DATABASE.Add(identifier, type);
			}
		}

		public static readonly Dictionary<string, Type> DATABASE = new Dictionary<string, Type>();
		public readonly string Identifier;

		public DynamicallyAccessibleEnumAttribute(string identifier)
		{
			Identifier = identifier;
		}
	}


	[Serializable]
	public class DynamicEnumValue
	{
		public string type = "";
		public byte byteValue = 0;
		public sbyte sByteValue = 0;
		public ushort uShortValue = 0;
		public short shortValue = 0;
		public uint uIntValue = 0;
		public int intValue = 0;
		public ulong uLongValue = 0;
		public long longValue = 0;

		[Serializable]
		public enum Type
		{
			Invalid,
			Byte,
			SignedByte,
			UnsignedShort,
			Short,
			UnsignedInt,
			Int,
			UnsignedLong,
			Long
		}

		public bool IsValid => DynamicallyAccessibleEnumAttribute.DATABASE.ContainsKey(type);

		public System.Type EnumType => DynamicallyAccessibleEnumAttribute.DATABASE[type];

		public Type EnumUnderlyingType
		{
			get
			{
				var enumType = DynamicallyAccessibleEnumAttribute.DATABASE[type];
				var enumUnderlyingType = Enum.GetUnderlyingType(enumType);

				if (enumUnderlyingType == typeof(byte))
					return Type.Byte;

				if (enumUnderlyingType == typeof(sbyte))
					return Type.SignedByte;

				if (enumUnderlyingType == typeof(ushort))
					return Type.UnsignedShort;

				if (enumUnderlyingType == typeof(short))
					return Type.Short;

				if (enumUnderlyingType == typeof(uint))
					return Type.UnsignedInt;

				if (enumUnderlyingType == typeof(int))
					return Type.Int;

				if (enumUnderlyingType == typeof(ulong))
					return Type.UnsignedLong;

				if (enumUnderlyingType == typeof(long))
					return Type.Long;

				return Type.Invalid;
			}
		}

		public byte MemorySize =>
			EnumUnderlyingType switch
			{
				Type.Byte => sizeof(byte),
				Type.SignedByte => sizeof(sbyte),
				Type.UnsignedShort => sizeof(ushort),
				Type.Short => sizeof(short),
				Type.UnsignedInt => sizeof(uint),
				Type.Int => sizeof(int),
				Type.UnsignedLong => sizeof(ulong),
				Type.Long => sizeof(long),
				Type.Invalid => throw new ArgumentOutOfRangeException(),
				_ => throw new ArgumentOutOfRangeException()
			};

		public unsafe void CopyValueTo(void* destinationAddress)
		{
			switch (EnumUnderlyingType)
			{
				case Type.Byte:
					*(byte*) destinationAddress = byteValue;
					break;
				case Type.SignedByte:
					*(sbyte*) destinationAddress = sByteValue;
					break;
				case Type.UnsignedShort:
					*(ushort*) destinationAddress = uShortValue;
					break;
				case Type.Short:
					*(short*) destinationAddress = shortValue;
					break;
				case Type.UnsignedInt:
					*(uint*) destinationAddress = uIntValue;
					break;
				case Type.Int:
					*(int*) destinationAddress = intValue;
					break;
				case Type.UnsignedLong:
					*(ulong*) destinationAddress = uLongValue;
					break;
				case Type.Long:
					*(long*) destinationAddress = longValue;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

#if UNITY_EDITOR
		public static Enum RuntimeCastToEnum<T>(T value, System.Type enumType)
		{
			var dataParam = Expression.Parameter(typeof(T), "data");
			var body = Expression.Block(Expression.Convert(dataParam, enumType));
			var run = Expression.Lambda(body, dataParam).Compile();
			var ret = (Enum) run.DynamicInvoke(value);
			return ret;
		}

		public static T RuntimeCastToUnderlyingType<T>(Enum input)
		{
			var dataParam = Expression.Parameter(input.GetType(), "data");
			var body = Expression.Block(Expression.Convert(dataParam, typeof(T)));
			var run = Expression.Lambda(body, dataParam).Compile();
			var ret = (T) run.DynamicInvoke(input);
			return ret;
		}

		public override string ToString() =>
			EnumUnderlyingType switch
			{
				Type.Byte => RuntimeCastToEnum(byteValue, EnumType).ToString(),
				Type.SignedByte => RuntimeCastToEnum(sByteValue, EnumType).ToString(),
				Type.UnsignedShort => RuntimeCastToEnum(uShortValue, EnumType).ToString(),
				Type.Short => RuntimeCastToEnum(shortValue, EnumType).ToString(),
				Type.UnsignedInt => RuntimeCastToEnum(uIntValue, EnumType).ToString(),
				Type.Int => RuntimeCastToEnum(intValue, EnumType).ToString(),
				Type.UnsignedLong => RuntimeCastToEnum(uLongValue, EnumType).ToString(),
				Type.Long => RuntimeCastToEnum(longValue, EnumType).ToString(),
				Type.Invalid => throw new ArgumentOutOfRangeException(),
				_ => throw new ArgumentOutOfRangeException()
			};
#endif
	}
}