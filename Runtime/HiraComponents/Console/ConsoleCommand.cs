using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
	internal class ConsoleCommand
	{
		private enum HiraConsoleCommandArgumentType : byte
		{
			Boolean,
			Byte,
			SByte,
			Short,
			UShort,
			Int,
			UInt,
			Long,
			ULong,
			Float,
			Double,
			String,
			Vector2,
			Vector3,
			Quaternion,
		}

		public ConsoleCommand(MethodInfo method, ParameterInfo[] parameters)
		{
			_method = method;
			byte i;
			var count = parameters.Length;
			for (i = 0; i < count; i++)
			{
				switch (i)
				{
					case 0: _arg1 = supported_types[parameters[i].ParameterType];
						break;
					case 1: _arg2 = supported_types[parameters[i].ParameterType];
						break;
					case 2: _arg3 = supported_types[parameters[i].ParameterType];
						break;
					case 3: _arg4 = supported_types[parameters[i].ParameterType];
						break;
					case 4: _arg5 = supported_types[parameters[i].ParameterType];
						break;
					case 5: _arg6 = supported_types[parameters[i].ParameterType];
						break;
					default: throw new ArgumentOutOfRangeException();
				}
			}

			_argumentCount = i;
		}

		public const byte MAX_SUPPORTED_ARGUMENTS = 5;

		private readonly MethodInfo _method;
		private readonly byte _argumentCount;
		private readonly HiraConsoleCommandArgumentType _arg1;
		private readonly HiraConsoleCommandArgumentType _arg2;
		private readonly HiraConsoleCommandArgumentType _arg3;
		private readonly HiraConsoleCommandArgumentType _arg4;
		private readonly HiraConsoleCommandArgumentType _arg5;
		private readonly HiraConsoleCommandArgumentType _arg6;

		private static readonly Dictionary<Type, HiraConsoleCommandArgumentType> supported_types =
			new Dictionary<Type, HiraConsoleCommandArgumentType>
			{
				{typeof(bool), HiraConsoleCommandArgumentType.Boolean},
				{typeof(byte), HiraConsoleCommandArgumentType.Byte},
				{typeof(sbyte), HiraConsoleCommandArgumentType.SByte},
				{typeof(short), HiraConsoleCommandArgumentType.Short},
				{typeof(ushort), HiraConsoleCommandArgumentType.UShort},
				{typeof(int), HiraConsoleCommandArgumentType.Int},
				{typeof(uint), HiraConsoleCommandArgumentType.UInt},
				{typeof(long), HiraConsoleCommandArgumentType.Long},
				{typeof(ulong), HiraConsoleCommandArgumentType.ULong},
				{typeof(float), HiraConsoleCommandArgumentType.Float},
				{typeof(double), HiraConsoleCommandArgumentType.Double},
				{typeof(string), HiraConsoleCommandArgumentType.String},
				{typeof(Vector2), HiraConsoleCommandArgumentType.Vector2},
				{typeof(Vector3), HiraConsoleCommandArgumentType.Vector3},
				{typeof(Quaternion), HiraConsoleCommandArgumentType.Quaternion},
			};

		public static bool IsTypeSupported(Type type) => supported_types.ContainsKey(type);

		public bool TryInvoke(string args)
		{
			var parsedArgs = args.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
			var parameters = new object[_argumentCount];

			if (parsedArgs.Length < _argumentCount)
				return false;

			for (byte i = 0; i < _argumentCount; i++)
			{
				if (!TryParse(parsedArgs[i], i, out var parsedParameter))
					return false;

				parameters[i] = parsedParameter;
			}

			_method.Invoke(null, parameters);
			return true;
		}

		private bool TryParse(string arg, byte index, out object output)
		{
			var argType = index switch
			{
				0 => _arg1,
				1 => _arg2,
				2 => _arg3,
				3 => _arg4,
				4 => _arg5,
				5 => _arg6,
				_ => throw new ArgumentOutOfRangeException()
			};

			switch (argType)
			{
				case HiraConsoleCommandArgumentType.Boolean:
					if (bool.TryParse(arg, out var result))
					{
						output = result;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.Byte:
					if (byte.TryParse(arg, out var byteResult))
					{
						output = byteResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.SByte:
					if (sbyte.TryParse(arg, out var sbyteResult))
					{
						output = sbyteResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.Short:
					if (short.TryParse(arg, out var shortResult))
					{
						output = shortResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.UShort:
					if (ushort.TryParse(arg, out var ushortResult))
					{
						output = ushortResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.Int:
					if (int.TryParse(arg, out var intResult))
					{
						output = intResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.UInt:
					if (uint.TryParse(arg, out var uintResult))
					{
						output = uintResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.Long:
					if (long.TryParse(arg, out var longResult))
					{
						output = longResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.ULong:
					if (ulong.TryParse(arg, out var ulongResult))
					{
						output = ulongResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.Float:
					if (float.TryParse(arg, out var floatResult))
					{
						output = floatResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.Double:
					if (double.TryParse(arg, out var doubleResult))
					{
						output = doubleResult;
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.String:
					output = arg;
					
					return true;
				case HiraConsoleCommandArgumentType.Vector2:
					var v2SplitArgs = arg.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
					if (v2SplitArgs.Length >= 2
					    && float.TryParse(v2SplitArgs[0], out var v2X)
					    && float.TryParse(v2SplitArgs[1], out var v2Y))
					{
						output = new Vector2(v2X, v2Y);
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.Vector3:
					var v3SplitArgs = arg.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
					if (v3SplitArgs.Length >= 3
					    && float.TryParse(v3SplitArgs[0], out var v3X)
					    && float.TryParse(v3SplitArgs[1], out var v3Y)
					    && float.TryParse(v3SplitArgs[2], out var v3Z))
					{
						output = new Vector3(v3X, v3Y, v3Z);
						return true;
					}

					break;
				case HiraConsoleCommandArgumentType.Quaternion:
					var qSplitArgs = arg.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
					if (qSplitArgs.Length >= 4
					    && float.TryParse(qSplitArgs[0], out var qX)
					    && float.TryParse(qSplitArgs[1], out var qY)
					    && float.TryParse(qSplitArgs[2], out var qZ)
					    && float.TryParse(qSplitArgs[3], out var qW))
					{
						output = new Quaternion(qX, qY, qZ, qW);
						return true;
					}

					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(argType), argType, null);
			}

			output = null;
			return false;
		}
	}
}