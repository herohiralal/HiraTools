namespace UnityEngine
{
    [AddComponentMenu("HiraTools/HiraCreatures/HiraCreature")]
    public class HiraCreature : MonoBehaviour
    {
        // TODO: Create a readme for this.
        [SerializeField] private HiraController controller;
        public HiraController Controller => controller;

        public virtual void InitializeAtSpawn<T>(in T spawnParameters)
        {
        }

        public virtual void Despawn()
        {
        }

        public virtual void OnPossess(HiraController inController)
        {
            controller = inController;
        }

        public virtual void OnUnpossess()
        {
            controller = null;
        }
    }
}