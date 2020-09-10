namespace UnityEngine
{
    public abstract class HiraCreatureComponent : MonoBehaviour, IHiraCreatureComponent
    {
        public virtual void InitializeAtSpawn<T>(in T spawnParameters)
        {
            
        }

        public virtual void Despawn()
        {
            
        }
    }
}