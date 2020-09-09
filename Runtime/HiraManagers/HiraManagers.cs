using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine
{
    public static class HiraManagers
    {
        private static readonly Dictionary<Type, Component> database = new Dictionary<Type, Component>();

        internal static void Clear() => database.Clear();

        public static bool Contains<T>() where T : Component => database.ContainsKey(typeof(T));

        public static T Get<T>() where T : Component
        {
            if (database.ContainsKey(typeof(T))) return (T) database[typeof(T)];
            
            Debug.LogErrorFormat($"{typeof(T).Name} is either not initialized yet" +
                                 $" or hasn't been marked as a manager.");
            return null;

        }

        internal static void Add(Type type, Component component)
        {
            Object.DontDestroyOnLoad(component);

            var propertyInfo = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            if (propertyInfo == null)
                propertyInfo = type.GetProperty("Current", BindingFlags.Public | BindingFlags.Static);
            if (propertyInfo != null) propertyInfo.SetValue(null, component);

            if (database.ContainsKey(type)) database[type] = component;
            else database.Add(type, component);
        }
    }
}