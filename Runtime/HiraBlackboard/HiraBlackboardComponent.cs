using UnityEngine;

namespace Hiralal.Blackboard
{
    internal class HiraBlackboardComponent
    {
        private readonly HiraBlackboardKeySet _keySet = null;
        
        internal readonly HiraBlackboardValueSet ValueSet = null;

        internal HiraBlackboardComponent(HiraBlackboardKeySet keySet, HiraBlackboardValueSet valueSet) => 
            (this._keySet, this.ValueSet) = (keySet, valueSet);

        private bool _synced = false;

        internal void RequestSynchronizationWithKeySet()
        {
            if (_synced) return;
            _synced = true;

            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateBoolean += OnSyncInstanceValueUpdate_boolean;
            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateFloat += OnSyncInstanceValueUpdate_float;
            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateInteger += OnSyncInstanceValueUpdate_integer;
            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateString += OnSyncInstanceValueUpdate_string;
            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateVector += OnSyncInstanceValueUpdate_vector;
        }

        internal void BreakSynchronizationWithKeySet()
        {
            if (!_synced) return;
            _synced = false;

            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateBoolean -= OnSyncInstanceValueUpdate_boolean;
            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateFloat -= OnSyncInstanceValueUpdate_float;
            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateInteger -= OnSyncInstanceValueUpdate_integer;
            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateString -= OnSyncInstanceValueUpdate_string;
            _keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateVector -= OnSyncInstanceValueUpdate_vector;
        }

        private void OnSyncInstanceValueUpdate_boolean(uint typeSpecificIndex, bool newValue) =>
            ValueSet.Booleans[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_float(uint typeSpecificIndex, float newValue) =>
            ValueSet.Floats[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_integer(uint typeSpecificIndex, int newValue) =>
            ValueSet.Integers[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_string(uint typeSpecificIndex, string newValue) =>
            ValueSet.Strings[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_vector(uint typeSpecificIndex, Vector3 newValue) =>
            ValueSet.Vectors[typeSpecificIndex] = newValue;
    }
}