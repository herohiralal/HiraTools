namespace UnityEngine
{
    public enum InitializationState
    {
        Inactive,
        Initializing,
        Active,
        ShuttingDown
    }
    
    public interface IInitializable
    {
        InitializationState InitializationStatus { get; }
        void Initialize();
        void Shutdown();
    }
}