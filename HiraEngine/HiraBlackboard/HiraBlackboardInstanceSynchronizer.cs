using System;
using UnityEngine;

namespace Hiralal.Blackboard
{
    internal sealed class HiraBlackboardInstanceSynchronizer
    {
        public HiraBlackboardInstanceSynchronizer(HiraBlackboardKeySet keySet) => this.keySet = keySet;

        private readonly HiraBlackboardKeySet keySet;
        
        internal event Action<uint, bool> OnSyncInstanceValueUpdateBoolean = delegate { };
        internal event Action<uint, float> OnSyncInstanceValueUpdateFloat = delegate { };
        internal event Action<uint, int> OnSyncInstanceValueUpdateInteger = delegate { };
        internal event Action<uint, string> OnSyncInstanceValueUpdateString = delegate { };
        internal event Action<uint, Vector3> OnSyncInstanceValueUpdateVector = delegate { };

        internal void ReportSyncedInstanceValueUpdate_boolean(uint hash, bool newValue) => 
            OnSyncInstanceValueUpdateBoolean(keySet.GetTypeSpecificIndex(hash), newValue);

        internal void ReportSyncedInstanceValueUpdate_float(uint hash, float newValue) => 
            OnSyncInstanceValueUpdateFloat(keySet.GetTypeSpecificIndex(hash), newValue);

        internal void ReportSyncedInstanceValueUpdate_integer(uint hash, int newValue) => 
            OnSyncInstanceValueUpdateInteger(keySet.GetTypeSpecificIndex(hash), newValue);

        internal void ReportSyncedInstanceValueUpdate_string(uint hash, string newValue) => 
            OnSyncInstanceValueUpdateString(keySet.GetTypeSpecificIndex(hash), newValue);

        internal void ReportSyncedInstanceValueUpdate_vector(uint hash, Vector3 newValue) => 
            OnSyncInstanceValueUpdateVector(keySet.GetTypeSpecificIndex(hash), newValue);
    }
}