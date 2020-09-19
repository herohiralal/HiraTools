using System;

namespace UnityEngine
{
    public class HiraBlackboard : HiraControllerComponent, IHiraControllerBlackboard
    {
        [SerializeField] private HiraBlackboardKeySet keySet = null;
        private IBlackboardValueAccessor _mainBlackboard = null;

        public override void Initialize<T>(in T spawnParameters)
        {
        }

        public override void OnPossess(HiraCreature inCreature)
        {
            if (keySet == null)
            {
                Debug.LogError($"Key set missing on controller for creature {inCreature.gameObject.name}.", this);
                return;
            }
            _mainBlackboard = BlackboardTypes.GetMainValueAccessor(keySet);
        }

        public override void OnDispossess()
        {
            _mainBlackboard = null;
        }

        public override void Stop()
        {
        }

        public IReadOnlyBlackboardDataSet DataSet => _mainBlackboard.DataSet;

        public event Action OnValueUpdate
        {
            add => _mainBlackboard.OnValueUpdate += value;
            remove => _mainBlackboard.OnValueUpdate -= value;
        }

        #region Getters
        public uint GetHash(in string keyName) => _mainBlackboard.GetHash(in keyName);
        public bool GetBooleanValue(uint hash) => _mainBlackboard.GetBooleanValue(hash);
        public float GetFloatValue(uint hash) => _mainBlackboard.GetFloatValue(hash);
        public int GetIntValue(uint hash) => _mainBlackboard.GetIntValue(hash);
        public string GetStringValue(uint hash) => _mainBlackboard.GetStringValue(hash);
        public Vector3 GetVectorValue(uint hash) => _mainBlackboard.GetVectorValue(hash);
        #endregion

        #region Setters
        public void SetBooleanValue(uint hash, bool value) => _mainBlackboard.SetBooleanValue(hash, value);
        public void SetFloatValue(uint hash, float value) => _mainBlackboard.SetFloatValue(hash, value);
        public void SetIntValue(uint hash, int value) => _mainBlackboard.SetIntValue(hash, value);
        public void SetStringValue(uint hash, string value) => _mainBlackboard.SetStringValue(hash, value);
        public void SetVectorValue(uint hash, Vector3 value) => _mainBlackboard.SetVectorValue(hash, value);
        #endregion
    }
}