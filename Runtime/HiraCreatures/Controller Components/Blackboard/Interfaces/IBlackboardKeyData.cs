namespace HiraCreatures.Components.Blackboard
{
    public interface IBlackboardKeyData
    {
        void Activate();
        void Deactivate();
        IReadOnlyInstanceSynchronizer InstanceSynchronizer { get; }
        uint GetHash(in string keyName);
        uint GetTypeSpecificIndex(uint hash);
        bool IsInstanceSynchronized(uint hash);
        UnityEngine.IBlackboardValueAccessor ValueAccessor { get; }
    }
}