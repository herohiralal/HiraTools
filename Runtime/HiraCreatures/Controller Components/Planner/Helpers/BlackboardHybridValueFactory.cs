using System;
using System.Collections.Generic;
using System.Linq;
using HiraEngine.Components.Planner;

namespace UnityEngine
{
    public static class BlackboardHybridValueFactory
    {
        static BlackboardHybridValueFactory()
        {
            bool_default_objects = BuildDictionary(false);
            float_default_objects = BuildDictionary(0.0f);
            int_default_objects = BuildDictionary(0);
            string_default_objects = BuildDictionary("");
            vector_default_objects = BuildDictionary(Vector3.zero);
        }

        // ReSharper disable once PossibleNullReferenceException
        private static Dictionary<string, IBlackboardHybridValueDefaultObject<T>> BuildDictionary<T>(T value)
        {
            var subclasses = typeof(IBlackboardHybridValueDefaultObject<T>).GetSubclasses();
            return subclasses
                .ToDictionary(ReflectionLibrary.GetReflectionName, t => GetDefaultObject(t, value));
        }

        private static readonly Dictionary<string, IBlackboardHybridValueDefaultObject<bool>> bool_default_objects;
        private static readonly Dictionary<string, IBlackboardHybridValueDefaultObject<float>> float_default_objects;
        private static readonly Dictionary<string, IBlackboardHybridValueDefaultObject<int>> int_default_objects;
        private static readonly Dictionary<string, IBlackboardHybridValueDefaultObject<string>> string_default_objects;
        private static readonly Dictionary<string, IBlackboardHybridValueDefaultObject<Vector3>> vector_default_objects;

        private static IBlackboardHybridValueDefaultObject<T> GetDefaultObject<T>(Type t, T value)
        {
            var constructorInfo = t.GetConstructor(new[] {typeof(uint), typeof(T)});
            
            if (constructorInfo != null)
                return (IBlackboardHybridValueDefaultObject<T>) constructorInfo.Invoke(new object[] {0u, value});
            
            Debug.LogError($"The constructor of type {t.FullName} does not match the required type.");
            return null;
        }

        public static IBlackboardHybridValue GetValue(string typeString, IBlackboardValueConstructorParams constructorParams)
        {
            if (string.IsNullOrWhiteSpace(typeString)) return null;
            
            if (bool_default_objects.ContainsKey(typeString))
                return bool_default_objects[typeString]
                    .GetNewHybridObject(constructorParams.TypeSpecificIndex, constructorParams.BoolValue);

            if (float_default_objects.ContainsKey(typeString))
                return float_default_objects[typeString]
                    .GetNewHybridObject(constructorParams.TypeSpecificIndex, constructorParams.FloatValue);

            if (int_default_objects.ContainsKey(typeString))
                return int_default_objects[typeString]
                    .GetNewHybridObject(constructorParams.TypeSpecificIndex, constructorParams.IntValue);

            if (string_default_objects.ContainsKey(typeString))
                return string_default_objects[typeString]
                    .GetNewHybridObject(constructorParams.TypeSpecificIndex, constructorParams.StringValue);

            if (vector_default_objects.ContainsKey(typeString))
                return vector_default_objects[typeString]
                    .GetNewHybridObject(constructorParams.TypeSpecificIndex, constructorParams.VectorValue);

            Debug.LogError($"Type string \"{typeString}\" does not match with any known types.");
            return null;
        }
    }
}