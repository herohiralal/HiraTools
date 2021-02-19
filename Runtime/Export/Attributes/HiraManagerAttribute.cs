using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class HiraManagerAttribute : Attribute
    {
        public HiraManagerAttribute() : this(null)
        {
        }
        
        public HiraManagerAttribute(string defaultPrefabLocation) => DefaultPrefabLocation = defaultPrefabLocation;
        public readonly string DefaultPrefabLocation;
        public byte Priority { get; set; } = 0;
    }

    public static class HiraManagers
    {
        private static readonly Dictionary<Type, Component> database = new Dictionary<Type, Component>();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnInitialization()
        {
            Application.quitting += Clear;

            var managerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(Component).IsAssignableFrom(t)))
                .Select(type => (type, type.GetCustomAttribute<HiraManagerAttribute>()))
                .Where(tuple => tuple.Item2 != null)
                .OrderByDescending(tuple => tuple.Item2.Priority);

            foreach (var (t, m) in managerTypes)
            {
                // checking if an instance is already present
                var alreadyPresent = Object.FindObjectOfType(t) as Component;
                if (alreadyPresent != null && alreadyPresent.GetType() == t)
                {
                    Add(t, alreadyPresent);
                    continue;
                }
                
                // checking if a default prefab is provided
                if (m.DefaultPrefabLocation != null)
                {
                    var prefab = Resources.Load<GameObject>(m.DefaultPrefabLocation);
                    if (prefab == null) prefab = Resources.Load<GameObject>($"Default_{m.DefaultPrefabLocation}");
                    if (prefab == null) Debug.LogErrorFormat($"Target prefab at {m.DefaultPrefabLocation} could not be found.");
                    else
                    {
                        var instantiatedObject = Object.Instantiate(prefab);
                        instantiatedObject.name = instantiatedObject.name.Replace("(Clone)", "");
                        var component = instantiatedObject.GetComponent(t);
                        // checking if the instantiated object has the component
                        if (component == null)
                        {
                            Debug.LogErrorFormat($"Target prefab at {m.DefaultPrefabLocation} does not contain " +
                                                 $"a {t.Name} component. Adding one to the instance.");
                            component = instantiatedObject.AddComponent(t);
                        }

                        Add(t, component);
                        continue;
                    }
                }

                var addedComponent = new GameObject($"[{t.Name}]").AddComponent(t);
                Add(t, addedComponent);
#if UNITY_EDITOR
                Debug.Log($"<color=green>Created HiraManager: </color>{addedComponent.gameObject.name}");
#endif
            }
        }

        private static void Clear()
        {
            Application.quitting -= Clear;
            
            var valueCollection = database.Values.Reverse();
            
            foreach (var component in valueCollection)
            {
                component.GetType().Assign("Instance", null);
                component.GetType().Assign("Current", null);

                if (component == null) continue;
                
#if UNITY_EDITOR
                Debug.Log($"<color=green>Destroying stale HiraManager: </color>{component.gameObject.name}");
#endif
                if (Application.isPlaying) Object.Destroy(component.gameObject);
                else Object.DestroyImmediate(component.gameObject);
            }

            database.Clear();
        }

        public static void Add(Type type, Component component)
        {
            Object.DontDestroyOnLoad(component);

            type.Assign("Instance", component);
            type.Assign("Current", component);

            if (database.ContainsKey(type)) database[type] = component;
            else database.Add(type, component);
        }

        public static T Get<T>() where T : Component
        {
            if (database.ContainsKey(typeof(T))) return (T) database[typeof(T)];
            
            Debug.LogErrorFormat($"{typeof(T).Name} is either not initialized yet" +
                                 $" or hasn't been marked as a manager.");
            return null;

        }
    }
}