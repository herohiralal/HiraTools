using UnityEngine;

namespace Hiralal.Blackboard
{
    internal class HiraBlackboardComponent
    {
        private readonly HiraBlackboardKeySet keySet = null;
        
        internal readonly HiraBlackboardValueSet valueSet = null;

        internal HiraBlackboardComponent(HiraBlackboardKeySet keySet, HiraBlackboardValueSet valueSet) => 
            (this.keySet, this.valueSet) = (keySet, valueSet);

        private bool synced = false;

        internal void RequestSynchronizationWithKeySet()
        {
            if (synced) return;
            synced = true;

            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateBoolean += OnSyncInstanceValueUpdate_boolean;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateFloat += OnSyncInstanceValueUpdate_float;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateInteger += OnSyncInstanceValueUpdate_integer;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateString += OnSyncInstanceValueUpdate_string;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateVector += OnSyncInstanceValueUpdate_vector;
        }

        internal void BreakSynchronizationWithKeySet()
        {
            if (!synced) return;
            synced = false;

            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateBoolean -= OnSyncInstanceValueUpdate_boolean;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateFloat -= OnSyncInstanceValueUpdate_float;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateInteger -= OnSyncInstanceValueUpdate_integer;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateString -= OnSyncInstanceValueUpdate_string;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateVector -= OnSyncInstanceValueUpdate_vector;
        }

        private void OnSyncInstanceValueUpdate_boolean(uint typeSpecificIndex, bool newValue) =>
            valueSet.booleans[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_float(uint typeSpecificIndex, float newValue) =>
            valueSet.floats[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_integer(uint typeSpecificIndex, int newValue) =>
            valueSet.integers[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_string(uint typeSpecificIndex, string newValue) =>
            valueSet.strings[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_vector(uint typeSpecificIndex, Vector3 newValue) =>
            valueSet.vectors[typeSpecificIndex] = newValue;
    }
}