namespace UnityEngine
{
    [AddComponentMenu("HiraTools/HiraCreatures/HiraCreature")]
    public class HiraCreature : MonoBehaviour
    {
        [SerializeField] private HiraController controller;
        public HiraController Controller => controller;

        public virtual void InitializeAtSpawn<T>(in T spawnParameters)
        {
        }

        public virtual void Kill()
        {
        }

        public virtual void OnPossess(HiraController inController)
        {
            controller = inController;
        }

        public virtual void OnDispossess()
        {
            controller = null;
        }

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