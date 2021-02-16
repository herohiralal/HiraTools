using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraEngine.SerializedInstance
{
    /// <summary>
    /// Get a singleton instance of a class from a string that defines the type.
    /// </summary>
    public static class SerializedInstanceFactory
    {
        static SerializedInstanceFactory() =>
            database = new Dictionary<string, object>();

        /// <summary>
        /// Caches the objects using string keys.
        /// </summary>
        private static readonly Dictionary<string, object> database;

        /// <summary>
        /// Get the singleton instance of the class
        /// </summary>
        /// <param name="typeString">String should specify full name of the class along with the namespace, and assembly.</param>
        /// <param name="cachedInstance">Would a cached instance work, or does the user need a completely new instance</param>
        /// <param name="shouldCache">Whether a created instance should be cached.</param>
        /// <typeparam name="T">The type of the required object.</typeparam>
        /// <returns></returns>
        public static T Get<T>(in string typeString, bool cachedInstance, bool shouldCache = true)
        {
            // check if an instance already exists in the database
            if (database.ContainsKey(typeString))
            {
                // check if the cached object can even be cast properly
                if (database[typeString] is T cachedObject)
                {
                    if (cachedInstance) return cachedObject;
                    else return (T) CreateInstance(cachedObject.GetType());
                }

                Debug.LogError(database[typeString].GetType().Name + " could not be cast as " + typeof(T).Name);
                return default;
            }

            // check if the string even returns a valid type
            var type = Type.GetType(typeString);
            if (type == null)
            {
                Debug.LogError("Could not derive a type from \"" + typeString + "\"");
                return default;
            }

            // if it can be cast as the requested type
            if (!typeof(T).IsAssignableFrom(type))
            {
                Debug.LogError(type.Name + " could not be cast as " + (typeof(T)).Name);
                return default;
            }

            var instance = CreateInstance(type);

            // if all the checks are passed, add the key-value pair to the database
            // and return the object
            if (shouldCache && !database.ContainsKey(typeString)) database.Add(typeString, instance);
            return (T) instance;
        }

        private static object CreateInstance(Type type)
        {
            object instance;

            // if it's a MonoBehaviour derived class
            if (typeof(MonoBehaviour).IsAssignableFrom(type))
                instance = new GameObject(type.Name).AddComponent(type);
            
            // if it's a ScriptableObject derived class
            else if (typeof(ScriptableObject).IsAssignableFrom(type))
                instance = ScriptableObject.CreateInstance(type);
            
            // if it's some other kind of Unity Object
            else if (typeof(Object).IsAssignableFrom(type))
                instance = null;
            
            // if it's a generic class
            else
                instance = Activator.CreateInstance(type);

            return instance;
        }
    }
}