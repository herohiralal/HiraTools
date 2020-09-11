using Hiralal.Blackboard;

namespace UnityEngine
{
    [AddComponentMenu("HiraTools/HiraBlackboard/Blackboard")]
    public class HiraBlackboard : MonoBehaviour
    {
        [SerializeField] private HiraBlackboardKeySet keySet = null;
        public HiraBlackboardKeySet KeySet => keySet;
        
        private HiraBlackboardComponent _state = null;

        internal HiraBlackboardValueSet ValueSet => _state.ValueSet;
        public HiraBlackboardValueSet GetDuplicateWorldState() => _state.ValueSet.Copy();

        private void Awake()
        {
            if (keySet != null)
            {
                _state = keySet.GetFreshBlackboardComponent();
                _state.RequestSynchronizationWithKeySet();
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

            _state?.BreakSynchronizationWithKeySet();
            _state = null;
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
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_boolean, _state.ValueSet.Booleans, hash);
        }

        // Floats
        public HiraBlackboardValueAccessor<float> GetFloatAccessor(in string keyName) =>
            GetFloatAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<float> GetFloatAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Float);

            return new HiraBlackboardValueAccessor<float>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_float, _state.ValueSet.Floats, hash);
        }

        // Integers
        public HiraBlackboardValueAccessor<int> GetIntegerAccessor(in string keyName) =>
            GetIntegerAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<int> GetIntegerAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Int);

            return new HiraBlackboardValueAccessor<int>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_integer, _state.ValueSet.Integers, hash);
        }

        // Strings
        public HiraBlackboardValueAccessor<string> GetStringAccessor(in string keyName) =>
            GetStringAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<string> GetStringAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.String);

            return new HiraBlackboardValueAccessor<string>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_string, _state.ValueSet.Strings, hash);
        }
        
        // Vectors
        public HiraBlackboardValueAccessor<Vector3> GetVectorAccessor(in string keyName) =>
            GetVectorAccessor(keySet.GetHash(in keyName));

        public HiraBlackboardValueAccessor<Vector3> GetVectorAccessor(uint hash)
        {
            keySet.ValidateTransaction(hash, HiraBlackboardKeyType.Vector);

            return new HiraBlackboardValueAccessor<Vector3>(keySet,
                keySet.InstanceSynchronizer.ReportSyncedInstanceValueUpdate_vector, _state.ValueSet.Vectors, hash);
        }
        
        #endregion
    }
}