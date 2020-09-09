using System;
using UnityEngine;

namespace Hiralal.Blackboard
{
    internal sealed class HiraBlackboardInstanceSynchronizer
    {
        public HiraBlackboardInstanceSynchronizer(HiraBlackboardKeySet keySet) => this._keySet = keySet;

        private readonly HiraBlackboardKeySet _keySet;
        
        internal event Action<uint, bool> OnSyncInstanceValueUpdateBoolean = delegate { };
        internal event Action<uint, float> OnSyncInstanceValueUpdateFloat = delegate { };
        internal event Action<uint, int> OnSyncInstanceValueUpdateInteger = delegate { };
        internal event Action<uint, string> OnSyncInstanceValueUpdateString = delegate { };
        internal event Action<uint, Vector3> OnSyncInstanceValueUpdateVector = delegate { };

        internal void ReportSyncedInstanceValueUpdate_boolean(uint hash, bool newValue) => 
            OnSyncInstanceValueUpdateBoolean(_keySet.GetTypeSpecificIndex(hash), newValue);

        internal void ReportSyncedInstanceValueUpdate_float(uint hash, float newValue) => 
            OnSyncInstanceValueUpdateFloat(_keySet.GetTypeSpecificIndex(hash), newValue);

        internal void ReportSyncedInstanceValueUpdate_integer(uint hash, int newValue) => 
            OnSyncInstanceValueUpdateInteger(_keySet.GetTypeSpecificIndex(hash), newValue);

        internal void ReportSyncedInstanceValueUpdate_string(uint hash, string newValue) => 
            OnSyncInstanceValueUpdateString(_keySet.GetTypeSpecificIndex(hash), newValue);

        internal void ReportSyncedInstanceValueUpdate_vector(uint hash, Vector3 newValue) => 
            OnSyncInstanceValueUpdateVector(_keySet.GetTypeSpecificIndex(hash), newValue);
    }
}