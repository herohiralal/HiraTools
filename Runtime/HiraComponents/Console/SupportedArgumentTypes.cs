using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
    internal partial class ConsoleCommand
    {
        private static readonly Dictionary<Type, ConsoleCommandArgumentType> supported_types =
            new Dictionary<Type, ConsoleCommandArgumentType>
            {
                {typeof(bool), ConsoleCommandArgumentType.Boolean},
                {typeof(byte), ConsoleCommandArgumentType.Byte},
                {typeof(sbyte), ConsoleCommandArgumentType.SByte},
                {typeof(short), ConsoleCommandArgumentType.Short},
                {typeof(ushort), ConsoleCommandArgumentType.UShort},
                {typeof(int), ConsoleCommandArgumentType.Int},
                {typeof(uint), ConsoleCommandArgumentType.UInt},
                {typeof(long), ConsoleCommandArgumentType.Long},
                {typeof(ulong), ConsoleCommandArgumentType.ULong},
                {typeof(float), ConsoleCommandArgumentType.Float},
                {typeof(double), ConsoleCommandArgumentType.Double},
                {typeof(string), ConsoleCommandArgumentType.String},
                {typeof(Vector2), ConsoleCommandArgumentType.Vector2},
                {typeof(Vector3), ConsoleCommandArgumentType.Vector3},
                {typeof(Quaternion), ConsoleCommandArgumentType.Quaternion},
            };
    }
}