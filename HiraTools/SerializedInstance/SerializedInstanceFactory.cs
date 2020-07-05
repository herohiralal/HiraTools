/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: SerializedInstanceFactory.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Get a singleton instance of a class from a string that defines the type.
 * *               Specialized for use with SerializedInstance.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiraBoilerplate.SerializedInstance
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
        /// <typeparam name="T">The type of the required object.</typeparam>
        /// <returns></returns>
        public static T Get<T>(in string typeString, bool cachedInstance)
        {
            // check if an instance already exists in the database
            if (database.ContainsKey(typeString))
            {
                // check if the cached object can even be cast properly
                if (database[typeString] is T cachedObject)
                {
                    if (cachedInstance) return cachedObject;
                    else return (T) Activator.CreateInstance(cachedObject.GetType());
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

            // check if the instance can even be cast as the required type
            var instance = Activator.CreateInstance(type);
            if (!(instance is T castedInstance))
            {
                Debug.LogError(type.Name + " could not be cast as " + (typeof(T)).Name);
                return default;
            }
            
            // if all the checks are passed, add the key-value pair to the database
            // and return the object
            database.Add(typeString, instance);
            return castedInstance;

        }
    }
}