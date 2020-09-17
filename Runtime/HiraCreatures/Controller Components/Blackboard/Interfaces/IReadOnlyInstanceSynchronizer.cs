namespace HiraCreatures.Components.Blackboard
{
    public delegate void OnBlackboardValueUpdate<in T>(uint typeSpecificIndex, T value);

    public interface IReadOnlyInstanceSynchronizer
    {
        event OnBlackboardValueUpdate<bool> OnSyncInstanceValueUpdateBoolean;
        event OnBlackboardValueUpdate<float> OnSyncInstanceValueUpdateFloat;
        event OnBlackboardValueUpdate<int> OnSyncInstanceValueUpdateInteger;
        event OnBlackboardValueUpdate<string> OnSyncInstanceValueUpdateString;
        event OnBlackboardValueUpdate<UnityEngine.Vector3> OnSyncInstanceValueUpdateVector;
    }
}