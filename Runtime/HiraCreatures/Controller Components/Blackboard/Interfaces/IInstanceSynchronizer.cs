using UnityEngine;

namespace HiraCreatures.Components.Blackboard
{
    public interface IInstanceSynchronizer : IReadOnlyInstanceSynchronizer
    {
        void ReportSyncedInstanceValueUpdate_boolean(uint typeSpecificIndex, bool newValue);
        void ReportSyncedInstanceValueUpdate_float(uint typeSpecificIndex, float newValue);
        void ReportSyncedInstanceValueUpdate_integer(uint typeSpecificIndex, int newValue);
        void ReportSyncedInstanceValueUpdate_string(uint typeSpecificIndex, string newValue);
        void ReportSyncedInstanceValueUpdate_vector(uint typeSpecificIndex, Vector3 newValue);
    }
}