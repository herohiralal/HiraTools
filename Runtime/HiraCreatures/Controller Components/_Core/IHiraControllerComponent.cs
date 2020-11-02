namespace UnityEngine
{
    public interface IHiraControllerComponent
    {
        void Initialize<T>(in T spawnParameters);
        void OnPossess(HiraCreature inCreature);
        void OnDispossess();
        void Stop();
        void DoGUI();
    }
}