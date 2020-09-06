using Hiralal.Blackboard;

namespace UnityEngine
{
    public class HiraBlackboard : MonoBehaviour, IHiraBlackboard
    {
        // TODO: Create a Readme for this.

        [SerializeField] private HiraBlackboardKeySet keySet = null;
        private HiraBlackboardValues state = null;

        private void Awake()
        {
            if (keySet != null)
            {
                state = new HiraBlackboardValues(keySet);
                state.RequestSynchronizationWithKeySet();
            }
            else
            {
                Debug.LogErrorFormat(gameObject, $"A blackboard needs a key set to function.");
                Destroy(this);
            }

            Awake_Override();
        }

        private void OnDestroy()
        {
            OnDestroy_Override();

            state?.BreakSynchronizationWithKeySet();
            state = null;
        }

        protected virtual void Awake_Override()
        {
        }

        protected virtual void OnDestroy_Override()
        {
        }

        #region BlackboardAccessors

        #region Boolean

        public bool GetValueAsBool(in string keyName) =>
            GetValueAsBool(keySet.GetHash(in keyName));

        public bool GetValueAsBool(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Bool);

            return state.booleans[keySet.GetTypeSpecificIndex(hash)];
        }

        public void SetValueAsBool(in string keyName, bool value) =>
            SetValueAsBool(keySet.GetHash(in keyName), value);

        public void SetValueAsBool(uint hash, bool value)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Bool);

            if (keySet.IsInstanceSynced(hash))
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_boolean(hash, value);
            else state.booleans[keySet.GetTypeSpecificIndex(hash)] = value;
        }

        #endregion

        #region Float

        public float GetValueAsFloat(in string keyName) =>
            GetValueAsFloat(keySet.GetHash(in keyName));

        public float GetValueAsFloat(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Float);

            return state.floats[keySet.GetTypeSpecificIndex(hash)];
        }

        public void SetValueAsFloat(in string keyName, float value) =>
            SetValueAsFloat(keySet.GetHash(in keyName), value);

        public void SetValueAsFloat(uint hash, float value)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Float);

            if (keySet.IsInstanceSynced(hash))
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_float(hash, value);
            else state.floats[keySet.GetTypeSpecificIndex(hash)] = value;
        }

        #endregion

        #region Int

        public int GetValueAsInt(in string keyName) =>
            GetValueAsInt(keySet.GetHash(in keyName));

        public int GetValueAsInt(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Int);

            return state.integers[keySet.GetTypeSpecificIndex(hash)];
        }

        public void SetValueAsInt(in string keyName, int value) =>
            SetValueAsInt(keySet.GetHash(in keyName), value);

        public void SetValueAsInt(uint hash, int value)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Int);

            if (keySet.IsInstanceSynced(hash))
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_integer(hash, value);
            else state.integers[keySet.GetTypeSpecificIndex(hash)] = value;
        }

        #endregion

        #region String

        public string GetValueAsString(in string keyName) =>
            GetValueAsString(keySet.GetHash(in keyName));

        public string GetValueAsString(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.String);

            return state.strings[keySet.GetTypeSpecificIndex(hash)];
        }

        public void SetValueAsString(in string keyName, string value) =>
            SetValueAsString(keySet.GetHash(in keyName), value);

        public void SetValueAsString(uint hash, string value)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.String);

            if (keySet.IsInstanceSynced(hash))
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_string(hash, value);
            else state.strings[keySet.GetTypeSpecificIndex(hash)] = value;
        }

        #endregion

        #region Vector

        public Vector3 GetValueAsVector(in string keyName) =>
            GetValueAsVector(keySet.GetHash(in keyName));

        public Vector3 GetValueAsVector(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Vector);

            return state.vectors[keySet.GetTypeSpecificIndex(hash)];
        }

        public void SetValueAsVector(in string keyName, Vector3 value) =>
            SetValueAsVector(keySet.GetHash(in keyName), value);

        public void SetValueAsVector(uint hash, Vector3 value)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Vector);

            if (keySet.IsInstanceSynced(hash))
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_vector(hash, value);
            else state.vectors[keySet.GetTypeSpecificIndex(hash)] = value;
        }

        #endregion

        #endregion
    }
}