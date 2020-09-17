using System;
using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal
{
    public class MainValueAccessor : IBlackboardValueAccessor
    {
        public MainValueAccessor(IBlackboardKeyData keyData)
        {
            var dataSet = keyData.ValueAccessor.DataSet.GetDuplicate();
            (_keyData, DataSet, _instanceSynchronizer) = (keyData, dataSet, keyData.InstanceSynchronizer);

            _instanceSynchronizer.OnSyncInstanceValueUpdateBoolean += ChangeBoolValue;
            _instanceSynchronizer.OnSyncInstanceValueUpdateFloat += ChangeFloatValue;
            _instanceSynchronizer.OnSyncInstanceValueUpdateInteger += ChangeIntValue;
            _instanceSynchronizer.OnSyncInstanceValueUpdateString += ChangeStringValue;
            _instanceSynchronizer.OnSyncInstanceValueUpdateVector += ChangeVectorValue;
        }

        ~MainValueAccessor()
        {
            _instanceSynchronizer.OnSyncInstanceValueUpdateVector -= ChangeVectorValue;
            _instanceSynchronizer.OnSyncInstanceValueUpdateString -= ChangeStringValue;
            _instanceSynchronizer.OnSyncInstanceValueUpdateInteger -= ChangeIntValue;
            _instanceSynchronizer.OnSyncInstanceValueUpdateFloat -= ChangeFloatValue;
            _instanceSynchronizer.OnSyncInstanceValueUpdateBoolean -= ChangeBoolValue;
        }

        private readonly IBlackboardKeyData _keyData;
        public IBlackboardDataSet DataSet { get; }
        private readonly IReadOnlyInstanceSynchronizer _instanceSynchronizer;
        public event Action OnValueUpdate = delegate { };

        public uint GetHash(in string keyName) => _keyData.GetHash(keyName);

        #region Getters

        public bool GetBooleanValue(uint hash) => DataSet.Booleans[_keyData.GetTypeSpecificIndex(hash)];

        public float GetFloatValue(uint hash) => DataSet.Floats[_keyData.GetTypeSpecificIndex(hash)];

        public int GetIntValue(uint hash) => DataSet.Integers[_keyData.GetTypeSpecificIndex(hash)];

        public string GetStringValue(uint hash) => DataSet.Strings[_keyData.GetTypeSpecificIndex(hash)];

        public Vector3 GetVectorValue(uint hash) => DataSet.Vectors[_keyData.GetTypeSpecificIndex(hash)];

        #endregion

        #region Setters

        public void SetBooleanValue(uint hash, bool value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetBooleanValue(hash, value);
            else ChangeBoolValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetFloatValue(uint hash, float value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetFloatValue(hash, value);
            else ChangeFloatValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetIntValue(uint hash, int value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetIntValue(hash, value);
            else ChangeIntValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetStringValue(uint hash, string value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetStringValue(hash, value);
            else ChangeStringValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetVectorValue(uint hash, Vector3 value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetVectorValue(hash, value);
            else ChangeVectorValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        #endregion

        #region Changers

        private void ChangeBoolValue(uint typeSpecificIndex, bool value)
        {
            DataSet.Booleans[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        private void ChangeFloatValue(uint typeSpecificIndex, float value)
        {
            DataSet.Floats[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        private void ChangeIntValue(uint typeSpecificIndex, int value)
        {
            DataSet.Integers[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        private void ChangeStringValue(uint typeSpecificIndex, string value)
        {
            DataSet.Strings[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        private void ChangeVectorValue(uint typeSpecificIndex, Vector3 value)
        {
            DataSet.Vectors[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        #endregion
    }
}