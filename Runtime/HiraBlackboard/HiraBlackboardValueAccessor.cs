using System;

namespace Hiralal.Blackboard
{
    public readonly struct HiraBlackboardValueAccessor<T>
    {
        internal HiraBlackboardValueAccessor(HiraBlackboardKeySet keySet, Action<uint, T> syncedInstanceValueUpdateReporter, T[] targetArray, uint hash, Action onValueSet)
        {
            _keySet = keySet;
            _syncedInstanceValueUpdateReporter = syncedInstanceValueUpdateReporter;
            _targetArray = targetArray;
            _hash = hash;
            _onValueSet = onValueSet;
        }

        private readonly HiraBlackboardKeySet _keySet;
        private readonly Action<uint, T> _syncedInstanceValueUpdateReporter;
        private readonly T[] _targetArray;
        private readonly uint _hash;
        private readonly Action _onValueSet;
        
        public T Value
        {
            get => _targetArray[_keySet.GetTypeSpecificIndex(_hash)];

            set
            {
                _onValueSet.Invoke();
                if (_keySet.IsInstanceSynced(_hash)) _syncedInstanceValueUpdateReporter.Invoke(_hash, value);
                else _targetArray[_keySet.GetTypeSpecificIndex(_hash)] = value;
            }
        }
    }
}