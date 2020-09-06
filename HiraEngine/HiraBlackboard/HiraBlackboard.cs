using Hiralal.Blackboard;

namespace UnityEngine
{
    public class HiraBlackboard : MonoBehaviour
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
        
        #region Blackboard Accessors

        // Booleans
        public HiraBlackboardValueAccessor<bool> GetBooleanAccessor(in string keyName) =>
            GetBooleanAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<bool> GetBooleanAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Bool);

            return new HiraBlackboardValueAccessor<bool>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_boolean, state.booleans, hash);
        }

        // Floats
        public HiraBlackboardValueAccessor<float> GetFloatAccessor(in string keyName) =>
            GetFloatAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<float> GetFloatAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Float);

            return new HiraBlackboardValueAccessor<float>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_float, state.floats, hash);
        }

        // Integers
        public HiraBlackboardValueAccessor<int> GetIntegerAccessor(in string keyName) =>
            GetIntegerAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<int> GetIntegerAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Int);

            return new HiraBlackboardValueAccessor<int>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_integer, state.integers, hash);
        }

        // Strings
        public HiraBlackboardValueAccessor<string> GetStringAccessor(in string keyName) =>
            GetStringAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<string> GetStringAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.String);

            return new HiraBlackboardValueAccessor<string>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_string, state.strings, hash);
        }
        
        // Vectors
        public HiraBlackboardValueAccessor<Vector3> GetVectorAccessor(in string keyName) =>
            GetVectorAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<Vector3> GetVectorAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Vector);

            return new HiraBlackboardValueAccessor<Vector3>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_vector, state.vectors, hash);
        }
        
        #endregion
    }
}