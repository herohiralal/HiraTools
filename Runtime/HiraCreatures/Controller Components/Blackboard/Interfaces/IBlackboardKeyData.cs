namespace HiraEngine.Components.Blackboard
{
    public interface IBlackboardKeyData
    {
        IReadOnlyInstanceSynchronizer InstanceSynchronizer { get; }
        uint GetHash(in string keyName);
        uint GetTypeSpecificIndex(uint hash);
        bool IsInstanceSynchronized(uint hash);
        UnityEngine.IBlackboardValueAccessor ValueAccessor { get; }
    }
}