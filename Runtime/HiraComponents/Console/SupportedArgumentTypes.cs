using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
    internal partial class ConsoleCommand
    {
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
    }
}