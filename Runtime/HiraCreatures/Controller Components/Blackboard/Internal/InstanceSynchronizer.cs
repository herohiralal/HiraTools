using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal
{
    public class InstanceSynchronizer : IInstanceSynchronizer
    {
        public event OnBlackboardValueUpdate<bool> OnSyncInstanceValueUpdateBoolean = delegate { };
        public event OnBlackboardValueUpdate<float> OnSyncInstanceValueUpdateFloat = delegate { };
        public event OnBlackboardValueUpdate<int> OnSyncInstanceValueUpdateInteger = delegate { };
        public event OnBlackboardValueUpdate<string> OnSyncInstanceValueUpdateString = delegate { };
        public event OnBlackboardValueUpdate<Vector3> OnSyncInstanceValueUpdateVector = delegate { };

        public void ReportSyncedInstanceValueUpdate_boolean(uint typeSpecificIndex, bool newValue) =>
            OnSyncInstanceValueUpdateBoolean(typeSpecificIndex, newValue);

        public void ReportSyncedInstanceValueUpdate_float(uint typeSpecificIndex, float newValue) =>
            OnSyncInstanceValueUpdateFloat(typeSpecificIndex, newValue);

        public void ReportSyncedInstanceValueUpdate_integer(uint typeSpecificIndex, int newValue) =>
            OnSyncInstanceValueUpdateInteger(typeSpecificIndex, newValue);

        public void ReportSyncedInstanceValueUpdate_string(uint typeSpecificIndex, string newValue) =>
            OnSyncInstanceValueUpdateString(typeSpecificIndex, newValue);

        public void ReportSyncedInstanceValueUpdate_vector(uint typeSpecificIndex, Vector3 newValue) =>
            OnSyncInstanceValueUpdateVector(typeSpecificIndex, newValue);
    }
}