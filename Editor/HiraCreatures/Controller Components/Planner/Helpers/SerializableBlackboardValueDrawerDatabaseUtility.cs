using System;
using System.Linq;
using HiraEngine.Components.Planner;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraEditor.HiraEngine.Components.Planner.Helpers
{
    public static class SerializableBlackboardValueDrawerDatabaseUtility
    {
        // Display
        
        // Query
        private static readonly string[] bool_query_display_names = GetQueryNames<bool>();
        private static readonly string[] float_query_display_names = GetQueryNames<float>();
        private static readonly string[] int_query_display_names = GetQueryNames<int>();
        private static readonly string[] string_query_display_names = GetQueryNames<string>();
        private static readonly string[] vector_query_display_names = GetQueryNames<Vector3>();
        
        // Modification
        private static readonly string[] bool_modification_display_names = GetModificationNames<bool>();
        private static readonly string[] float_modification_display_names = GetModificationNames<float>();
        private static readonly string[] int_modification_display_names = GetModificationNames<int>();
        private static readonly string[] string_modification_display_names = GetModificationNames<string>();
        private static readonly string[] vector_modification_display_names = GetModificationNames<Vector3>();
        
        // Hybrid
        private static readonly string[] bool_hybrid_display_names = GetHybridNames<bool>();
        private static readonly string[] float_hybrid_display_names = GetHybridNames<float>();
        private static readonly string[] int_hybrid_display_names = GetHybridNames<int>();
        private static readonly string[] string_hybrid_display_names = GetHybridNames<string>();
        private static readonly string[] vector_hybrid_display_names = GetHybridNames<Vector3>();

        // Reflection
        
        // Query
        private static readonly string[] bool_query_reflection_names = GetQueryReflectionNames<bool>();
        private static readonly string[] float_query_reflection_names = GetQueryReflectionNames<float>();
        private static readonly string[] int_query_reflection_names = GetQueryReflectionNames<int>();
        private static readonly string[] string_query_reflection_names = GetQueryReflectionNames<string>();
        private static readonly string[] vector_query_reflection_names = GetQueryReflectionNames<Vector3>();
        
        // Modification
        private static readonly string[] bool_modification_reflection_names = GetModificationReflectionNames<bool>();
        private static readonly string[] float_modification_reflection_names = GetModificationReflectionNames<float>();
        private static readonly string[] int_modification_reflection_names = GetModificationReflectionNames<int>();
        private static readonly string[] string_modification_reflection_names = GetModificationReflectionNames<string>();
        private static readonly string[] vector_modification_reflection_names = GetModificationReflectionNames<Vector3>();
        
        // Hybrid
        private static readonly string[] bool_hybrid_reflection_names = GetHybridReflectionNames<bool>();
        private static readonly string[] float_hybrid_reflection_names = GetHybridReflectionNames<float>();
        private static readonly string[] int_hybrid_reflection_names = GetHybridReflectionNames<int>();
        private static readonly string[] string_hybrid_reflection_names = GetHybridReflectionNames<string>();
        private static readonly string[] vector_hybrid_reflection_names = GetHybridReflectionNames<Vector3>();

        private static string[] GetQueryNames<T>() => GetNames(typeof(IBlackboardQueryDefaultObject<T>));
        private static string[] GetModificationNames<T>() => GetNames(typeof(IBlackboardModificationDefaultObject<T>));
        private static string[] GetHybridNames<T>() => GetNames(typeof(IBlackboardHybridValueDefaultObject<T>));
        private static string[] GetNames(Type t) => t.GetSubclasses().Select(GetDisplayName).ToArray();

        private static string GetDisplayName(this Type t) =>
            t.Name
                .Replace("Bool", "")
                .Replace("Float", "")
                .Replace("Int", "")
                .Replace("String", "")
                .Replace("Vector", "")
                .Replace("Value", "");

        private static string[] GetQueryReflectionNames<T>() => GetReflectionNames(typeof(IBlackboardQueryDefaultObject<T>));
        private static string[] GetModificationReflectionNames<T>() => GetReflectionNames(typeof(IBlackboardModificationDefaultObject<T>));
        private static string[] GetHybridReflectionNames<T>() => GetReflectionNames(typeof(IBlackboardHybridValueDefaultObject<T>));
        private static string[] GetReflectionNames(Type t) =>
            t.GetSubclasses().Select(ReflectionLibrary.GetReflectionName).ToArray();

        public static (string[], SerializableBlackboardKey[]) BuildKeyData(this Object keySet)
        {
            if (!(keySet is HiraBlackboardKeySet hiraKeySet)) return (null, null);

            var keys = hiraKeySet.Keys;
            if (keys.Length == 0) return (null, null);
            var names = keys.Select(k => k.Name).ToArray();
            return (names, keys);
        }

        public static (string[], string[]) BuildQueryCalculationData(this SerializableBlackboardKey sKey)
        {
            switch (sKey.KeyType)
            {
                case BlackboardKeyType.Bool:
                    return (bool_query_display_names, bool_query_reflection_names);
                case BlackboardKeyType.Float:
                    return (float_query_display_names, float_query_reflection_names);
                case BlackboardKeyType.Int:
                    return (int_query_display_names, int_query_reflection_names);
                case BlackboardKeyType.String:
                    return (string_query_display_names, string_query_reflection_names);
                case BlackboardKeyType.Vector:
                    return (vector_query_display_names, vector_query_reflection_names);
                default:
                    return (null, null);
            }
        }

        public static (string[], string[]) BuildModificationCalculationData(this SerializableBlackboardKey sKey)
        {
            switch (sKey.KeyType)
            {
                case BlackboardKeyType.Bool:
                    return (bool_modification_display_names, bool_modification_reflection_names);
                case BlackboardKeyType.Float:
                    return (float_modification_display_names, float_modification_reflection_names);
                case BlackboardKeyType.Int:
                    return (int_modification_display_names, int_modification_reflection_names);
                case BlackboardKeyType.String:
                    return (string_modification_display_names, string_modification_reflection_names);
                case BlackboardKeyType.Vector:
                    return (vector_modification_display_names, vector_modification_reflection_names);
                default:
                    return (null, null);
            }
        }

        public static (string[], string[]) BuildHybridCalculationData(this SerializableBlackboardKey sKey)
        {
            switch (sKey.KeyType)
            {
                case BlackboardKeyType.Bool:
                    return (bool_hybrid_display_names, bool_hybrid_reflection_names);
                case BlackboardKeyType.Float:
                    return (float_hybrid_display_names, float_hybrid_reflection_names);
                case BlackboardKeyType.Int:
                    return (int_hybrid_display_names, int_hybrid_reflection_names);
                case BlackboardKeyType.String:
                    return (string_hybrid_display_names, string_hybrid_reflection_names);
                case BlackboardKeyType.Vector:
                    return (vector_hybrid_display_names, vector_hybrid_reflection_names);
                default:
                    return (null, null);
            }
        }
    }
}