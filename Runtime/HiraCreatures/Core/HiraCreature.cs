using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    [AddComponentMenu("HiraTools/HiraCreatures/HiraCreature")]
    public abstract class HiraCreature : MonoBehaviour, IEnumerable<IHiraCreatureComponent>
    {
        [SerializeField] private HiraController controller;
        public HiraController Controller => controller;

        public abstract void InitializeAtSpawn<T>(in T spawnParameters);

        public abstract void Kill();

        public virtual void OnPossess(HiraController inController)
        {
            controller = inController;
        }

        public virtual void OnDispossess()
        {
            controller = null;
        }

        public abstract IEnumerator<IHiraCreatureComponent> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if UNITY_EDITOR
        [SerializeField] private HiraControllerTemplate controllerTemplate = null;
#endif

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (controllerTemplate != null)
            {
                if (Application.isPlaying) controllerTemplate.Possess(this);
                controllerTemplate = null;
            }
#endif
        }
    }
}