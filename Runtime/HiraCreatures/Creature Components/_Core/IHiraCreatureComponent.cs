namespace UnityEngine
{
    public interface IHiraCreatureComponent
    {
        void InitializeAtSpawn<T>(in T spawnParameters);
        void OnPossess(HiraController inController);
        void OnDispossess();
        void Kill();
    }
}