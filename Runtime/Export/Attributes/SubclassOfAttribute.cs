using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SubclassOfAttribute : PropertyAttribute
    {
        public SubclassOfAttribute(Type type) => Type = type;
        public readonly Type Type;
    }

    public static class SubclassFactory
    {
        private static System.Collections.Generic.Dictionary<string, object> _database = new System.Collections.Generic.Dictionary<string, object>();

        [RuntimeInitializeOnLoadMethod]
        private static void ResetDatabase()
        {
            var objects = _database.Values;
            foreach (var o in objects)
            {
                Action<Object> destroyer;
                if (Application.isPlaying) destroyer = Object.Destroy;
                else destroyer = Object.DestroyImmediate;

                // if it's a MonoBehaviour derived class
                if (o is MonoBehaviour mb && mb != null)
                    destroyer.Invoke(mb);

                // if it's a ScriptableObject derived class
                else if (o is ScriptableObject so && so != null)
                    destroyer.Invoke(so);

                // if it's some other kind of Unity Object
                else if (o is Object uo && uo != null)
                    destroyer.Invoke(uo);

                // if it's a simple class
                else { }
            }

            _database.Clear();
            _database = new System.Collections.Generic.Dictionary<string, object>();
        }
        
        public static T Get<T>(in string typeString, bool cachedInstance, bool shouldCache = true) where T : new()
        {
            // check if an instance already exists in the database
            if (_database.ContainsKey(typeString))
            {
                // check if the cached object can even be cast properly
                if (_database[typeString] is T cachedObject)
                {
                    if (cachedInstance) return cachedObject;
                    else return (T) CreateInstance(cachedObject.GetType());
                }

                Debug.LogError(_database[typeString].GetType().Name + " could not be cast as " + typeof(T).Name);
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
            if (shouldCache && !_database.ContainsKey(typeString)) _database.Add(typeString, instance);
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
            
            // if it's a simple class
            else
                instance = Activator.CreateInstance(type);

            return instance;
        }
    }
}