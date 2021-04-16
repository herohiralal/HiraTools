using System;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
    internal static class ParsingUtility
    {
        internal static void ParseCommandline(string commandline, out string command, out string args)
        {
            var parsedCommand = commandline.Split(new[] {' '}, 2, StringSplitOptions.RemoveEmptyEntries);
            command = parsedCommand[0].ToLower();
            args = parsedCommand.Length > 1 ? parsedCommand[1] : "";
        }

        internal static bool TryParse(string arg, ConsoleCommandArgumentType argType, out object output)
		{
            switch (argType)
			{
				case ConsoleCommandArgumentType.Boolean:
					if (bool.TryParse(arg, out var result))
					{
						output = result;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.Byte:
					if (byte.TryParse(arg, out var byteResult))
					{
						output = byteResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.SByte:
					if (sbyte.TryParse(arg, out var sbyteResult))
					{
						output = sbyteResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.Short:
					if (short.TryParse(arg, out var shortResult))
					{
						output = shortResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.UShort:
					if (ushort.TryParse(arg, out var ushortResult))
					{
						output = ushortResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.Int:
					if (int.TryParse(arg, out var intResult))
					{
						output = intResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.UInt:
					if (uint.TryParse(arg, out var uintResult))
					{
						output = uintResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.Long:
					if (long.TryParse(arg, out var longResult))
					{
						output = longResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.ULong:
					if (ulong.TryParse(arg, out var ulongResult))
					{
						output = ulongResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.Float:
					if (float.TryParse(arg, out var floatResult))
					{
						output = floatResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.Double:
					if (double.TryParse(arg, out var doubleResult))
					{
						output = doubleResult;
						return true;
					}

					break;
				case ConsoleCommandArgumentType.String:
					output = arg;
					
					return true;
				case ConsoleCommandArgumentType.Vector2:
					var v2SplitArgs = arg.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
					if (v2SplitArgs.Length >= 2
					    && float.TryParse(v2SplitArgs[0], out var v2X)
					    && float.TryParse(v2SplitArgs[1], out var v2Y))
					{
						output = new Vector2(v2X, v2Y);
						return true;
					}

					break;
				case ConsoleCommandArgumentType.Vector3:
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
				case ConsoleCommandArgumentType.Quaternion:
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