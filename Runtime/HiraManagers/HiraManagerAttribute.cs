using System;
using System.Linq;
using System.Reflection;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class HiraManagerAttribute : Attribute
    {

        public HiraManagerAttribute() => defaultPrefabLocation = null;
        public HiraManagerAttribute(string defaultPrefabLocation) => this.defaultPrefabLocation = defaultPrefabLocation;
        private readonly string defaultPrefabLocation;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnInitialization()
        {
            HiraManagers.Clear();

            var managerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(Component).IsAssignableFrom(t)))
                .Select(type => (type, type.GetCustomAttribute<HiraManagerAttribute>()))
                .Where(tuple => tuple.Item2 != null);

            foreach (var (t, m) in managerTypes)
            {
                // checking if an instance is already present
                var alreadyPresent = Object.FindObjectOfType(t) as Component;
                if (alreadyPresent != null && alreadyPresent.GetType() == t)
                {
                    HiraManagers.Add(t, alreadyPresent);
                    continue;
                }
                
                // checking if a default prefab is provided
                if (m.defaultPrefabLocation != null)
                {
                    var prefab = Resources.Load<GameObject>(m.defaultPrefabLocation);
                    if (prefab == null) prefab = Resources.Load<GameObject>($"Default_{m.defaultPrefabLocation}");
                    if (prefab == null) Debug.LogErrorFormat($"Target prefab at {m.defaultPrefabLocation} could not be found.");
                    else
                    {
                        var instantiatedObject = Object.Instantiate(prefab);
                        instantiatedObject.name = instantiatedObject.name.Replace("(Clone)", "");
                        var component = instantiatedObject.GetComponent(t);
                        // checking if the instantiated object has the component
                        if (component == null)
                        {
                            Debug.LogErrorFormat($"Target prefab at {m.defaultPrefabLocation} does not contain " +
                                                 $"a {t.Name} component. Adding one to the instance.");
                            component = instantiatedObject.AddComponent(t);
                        }

                        HiraManagers.Add(t, component);
                        continue;
                    }
                }

                var addedComponent = new GameObject($"[{t.Name}]").AddComponent(t);
                HiraManagers.Add(t, addedComponent);
            }
        }
    }
}