namespace UnityEngine
{
    public interface IInitializable
    {
        void Initialize<T>(ref T initParams);
        void Shutdown();
    }
}