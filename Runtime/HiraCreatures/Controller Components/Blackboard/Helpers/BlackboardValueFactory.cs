using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Helpers
{
    public static class BlackboardValueFactory
    {
        static BlackboardValueFactory()
        {
            bool_value_default_objects = BuildDictionary(false);
            float_value_default_objects = BuildDictionary(0.0f);
            int_value_default_objects = BuildDictionary(0);
            string_value_default_objects = BuildDictionary("");
            vector_value_default_objects = BuildDictionary(Vector3.zero);
        }

        // ReSharper disable once PossibleNullReferenceException
        private static Dictionary<string, IBlackboardValueDefaultObject<T>> BuildDictionary<T>(T value)
        {
            var subclasses = typeof(IBlackboardValueDefaultObject<T>).GetSubclasses();
            return subclasses
                .ToDictionary(TypeExtensions.GetReflectionName, t => GetDefaultObject(t, value));
        }

        private static readonly Dictionary<string, IBlackboardValueDefaultObject<bool>> bool_value_default_objects;
        private static readonly Dictionary<string, IBlackboardValueDefaultObject<float>> float_value_default_objects;
        private static readonly Dictionary<string, IBlackboardValueDefaultObject<int>> int_value_default_objects;
        private static readonly Dictionary<string, IBlackboardValueDefaultObject<string>> string_value_default_objects;
        private static readonly Dictionary<string, IBlackboardValueDefaultObject<Vector3>> vector_value_default_objects;

        private static IBlackboardValueDefaultObject<T> GetDefaultObject<T>(Type t, T value)
        {
            var constructorInfo = t.GetConstructor(new[] {typeof(uint), typeof(T)});
            
            if (constructorInfo != null)
                return (IBlackboardValueDefaultObject<T>) constructorInfo.Invoke(new object[] {0u, value});
            
            Debug.LogError($"The constructor of type {t.FullName} does not match the required type.");
            return null;
        }

        public static IBlackboardValue GetValue(string typeString, IBlackboardValueConstructorParams constructorParams)
        {
            if (bool_value_default_objects.ContainsKey(typeString))
                return bool_value_default_objects[typeString]
                    .GetNewObject(constructorParams.TypeSpecificIndex, constructorParams.BoolValue);

            if (float_value_default_objects.ContainsKey(typeString))
                return float_value_default_objects[typeString]
                    .GetNewObject(constructorParams.TypeSpecificIndex, constructorParams.FloatValue);

            if (int_value_default_objects.ContainsKey(typeString))
                return int_value_default_objects[typeString]
                    .GetNewObject(constructorParams.TypeSpecificIndex, constructorParams.IntValue);

            if (string_value_default_objects.ContainsKey(typeString))
                return string_value_default_objects[typeString]
                    .GetNewObject(constructorParams.TypeSpecificIndex, constructorParams.StringValue);

            if (vector_value_default_objects.ContainsKey(typeString))
                return vector_value_default_objects[typeString]
                    .GetNewObject(constructorParams.TypeSpecificIndex, constructorParams.VectorValue);

            Debug.LogError($"Type string \"{typeString}\" does not match with any known types.");
            return null;
        }
    }
}