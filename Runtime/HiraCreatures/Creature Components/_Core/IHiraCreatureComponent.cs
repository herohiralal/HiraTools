namespace UnityEngine
{
    public interface IHiraCreatureComponent
    {
        void InitializeAtSpawn<T>(in T spawnParameters);
        void Despawn();
    }
}