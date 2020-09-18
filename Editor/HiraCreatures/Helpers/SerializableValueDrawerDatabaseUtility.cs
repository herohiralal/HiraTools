using System;
using System.Linq;
using HiraCreatures.Components.Blackboard.Internal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraCreatures.Components.Blackboard.Editor.Helpers
{
    public static class SerializableValueDrawerDatabaseUtility
    {
        private static readonly string[] bool_display_names = GetNames<bool>();
        private static readonly string[] float_display_names = GetNames<float>();
        private static readonly string[] int_display_names = GetNames<int>();
        private static readonly string[] string_display_names = GetNames<string>();
        private static readonly string[] vector_display_names = GetNames<Vector3>();

        private static readonly string[] bool_reflection_names = GetReflectionNames<bool>();
        private static readonly string[] float_reflection_names = GetReflectionNames<float>();
        private static readonly string[] int_reflection_names = GetReflectionNames<int>();
        private static readonly string[] string_reflection_names = GetReflectionNames<string>();
        private static readonly string[] vector_reflection_names = GetReflectionNames<Vector3>();

        private static string[] GetNames<T>() =>
            typeof(IBlackboardValueDefaultObject<T>).GetSubclasses().Select(GetDisplayName).ToArray();

        private static string GetDisplayName(this Type t) =>
            t.Name
                .Replace("Bool", "")
                .Replace("Float", "")
                .Replace("Int", "")
                .Replace("String", "")
                .Replace("Vector", "")
                .Replace("Value", "");

        private static string[] GetReflectionNames<T>() => 
            typeof(IBlackboardValueDefaultObject<T>).GetSubclasses().Select(TypeExtensions.GetReflectionName).ToArray();

        public static (string[], SerializableKey[]) BuildKeyData(this Object keySet)
        {
            if (!(keySet is HiraBlackboardKeySet hiraKeySet)) return (null, null);
            
            var keys = hiraKeySet.Keys;
            if (keys.Length == 0) return (null, null);
            var names = keys.Select(k => k.Name).ToArray();
            return (names, keys);
        }

        public static (string[], string[]) BuildCalculationData(this Object key)
        {
            if (!(key is SerializableKey sKey)) return (null, null);
            
            switch (sKey.KeyType)
            {
                case BlackboardKeyType.Bool:
                    return (bool_display_names, bool_reflection_names);
                case BlackboardKeyType.Float:
                    return (float_display_names, float_reflection_names);
                case BlackboardKeyType.Int:
                    return (int_display_names, int_reflection_names);
                case BlackboardKeyType.String:
                    return (string_display_names, string_reflection_names);
                case BlackboardKeyType.Vector:
                    return (vector_display_names, vector_reflection_names);
                default:
                    return (null, null);
            }
        }
    }
}