using System;

namespace Hiralal.Blackboard
{
    public readonly struct HiraBlackboardValueAccessor<T>
    {
        internal HiraBlackboardValueAccessor(HiraBlackboardKeySet keySet, Action<uint, T> syncedInstanceValueUpdateReporter, T[] targetArray, uint hash)
        {
            this.keySet = keySet;
            this.syncedInstanceValueUpdateReporter = syncedInstanceValueUpdateReporter;
            this.targetArray = targetArray;
            this.hash = hash;
        }

        private readonly HiraBlackboardKeySet keySet;
        private readonly Action<uint, T> syncedInstanceValueUpdateReporter;
        private readonly T[] targetArray;
        private readonly uint hash;
        
        public T Value
        {
            get => targetArray[keySet.GetTypeSpecificIndex(hash)];

            set
            {
                if (keySet.IsInstanceSynced(hash)) syncedInstanceValueUpdateReporter.Invoke(hash, value);
                else targetArray[keySet.GetTypeSpecificIndex(hash)] = value;
            }
        }
    }
}