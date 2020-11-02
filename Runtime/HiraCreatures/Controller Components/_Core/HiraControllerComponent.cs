namespace UnityEngine
{
    public abstract class HiraControllerComponent : MonoBehaviour, IHiraControllerComponent
    {
        public virtual void Initialize<T>(in T spawnParameters)
        {
        }

        public virtual void OnPossess(HiraCreature inCreature)
        {
        }

        public virtual void OnDispossess()
        {
        }

        public virtual void Stop()
        {
        }

        public abstract void DoGUI();
    }
}