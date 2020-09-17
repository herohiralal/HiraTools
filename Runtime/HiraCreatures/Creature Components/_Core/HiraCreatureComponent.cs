namespace UnityEngine
{
    public abstract class HiraCreatureComponent : MonoBehaviour, IHiraCreatureComponent
    {
        public virtual void InitializeAtSpawn<T>(in T spawnParameters)
        {
            
        }

        public virtual void OnPossess(HiraController inController)
        {
            
        }

        public virtual void OnDispossess()
        {
            
        }

        public virtual void Kill()
        {
            
        }
    }
}