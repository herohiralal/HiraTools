using System;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
    public class MainValueAccessor : IBlackboardValueAccessor
    {
        public MainValueAccessor(IBlackboardKeyData keyData)
        {
            var dataSet = keyData.ValueAccessor.DataSet.GetDuplicate();
            (_keyData, _dataSet, _instanceSynchronizer) = (keyData, dataSet, keyData.InstanceSynchronizer);

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
        private readonly IReadWriteBlackboardDataSet _dataSet;
        private readonly IReadOnlyInstanceSynchronizer _instanceSynchronizer;

        public IReadOnlyBlackboardDataSet DataSet => _dataSet;
        public event Action OnValueUpdate = delegate { };

        public uint GetHash(in string keyName) => _keyData.GetHash(keyName);

        public void Reset() => _keyData.ValueAccessor.DataSet.CopyTo(_dataSet);

        #region Getters

        public bool GetBooleanValue(uint hash) => GetBooleanValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash));
        public bool GetBooleanValueWithTypeSpecificIndex(uint typeSpecificIndex) => _dataSet.Booleans[typeSpecificIndex];
        public float GetFloatValue(uint hash) => GetFloatValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash));
        public float GetFloatValueWithTypeSpecificIndex(uint typeSpecificIndex) => _dataSet.Floats[typeSpecificIndex];
        public int GetIntValue(uint hash) => GetIntValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash));
        public int GetIntValueWithTypeSpecificIndex(uint typeSpecificIndex) => _dataSet.Integers[typeSpecificIndex];
        public string GetStringValue(uint hash) => GetStringValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash));
        public string GetStringValueWithTypeSpecificIndex(uint typeSpecificIndex) => _dataSet.Strings[typeSpecificIndex];
        public Vector3 GetVectorValue(uint hash) => GetVectorValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash));
        public Vector3 GetVectorValueWithTypeSpecificIndex(uint typeSpecificIndex) => _dataSet.Vectors[typeSpecificIndex];

        #endregion

        #region Setters

        public void SetBooleanValue(uint hash, bool value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetBooleanValue(hash, value);
            else ChangeBoolValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetBooleanValueWithTypeSpecificIndex(uint typeSpecificIndex, bool value)
        {
            if (_keyData.IsBooleanKeyInstanceSynchronized(typeSpecificIndex))
                _keyData.ValueAccessor.SetBooleanValueWithTypeSpecificIndex(typeSpecificIndex, value);
            else ChangeBoolValue(typeSpecificIndex, value);
        }

        public void SetFloatValue(uint hash, float value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetFloatValue(hash, value);
            else ChangeFloatValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetFloatValueWithTypeSpecificIndex(uint typeSpecificIndex, float value)
        {
            if (_keyData.IsFloatKeyInstanceSynchronized(typeSpecificIndex))
                _keyData.ValueAccessor.SetFloatValueWithTypeSpecificIndex(typeSpecificIndex, value);
            else ChangeFloatValue(typeSpecificIndex, value);
        }

        public void SetIntValue(uint hash, int value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetIntValue(hash, value);
            else ChangeIntValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetIntValueWithTypeSpecificIndex(uint typeSpecificIndex, int value)
        {
            if (_keyData.IsIntKeyInstanceSynchronized(typeSpecificIndex))
                _keyData.ValueAccessor.SetIntValueWithTypeSpecificIndex(typeSpecificIndex, value);
            else ChangeIntValue(typeSpecificIndex, value);
        }

        public void SetStringValue(uint hash, string value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetStringValue(hash, value);
            else ChangeStringValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetStringValueWithTypeSpecificIndex(uint typeSpecificIndex, string value)
        {
            if (_keyData.IsStringKeyInstanceSynchronized(typeSpecificIndex))
                _keyData.ValueAccessor.SetStringValueWithTypeSpecificIndex(typeSpecificIndex, value);
            else ChangeStringValue(typeSpecificIndex, value);
        }

        public void SetVectorValue(uint hash, Vector3 value)
        {
            if (_keyData.IsInstanceSynchronized(hash)) _keyData.ValueAccessor.SetVectorValue(hash, value);
            else ChangeVectorValue(_keyData.GetTypeSpecificIndex(hash), value);
        }

        public void SetVectorValueWithTypeSpecificIndex(uint typeSpecificIndex, Vector3 value)
        {
            if (_keyData.IsVectorKeyInstanceSynchronized(typeSpecificIndex))
                _keyData.ValueAccessor.SetVectorValueWithTypeSpecificIndex(typeSpecificIndex, value);
            else ChangeVectorValue(typeSpecificIndex, value);
        }

        #endregion

        #region Changers

        private void ChangeBoolValue(uint typeSpecificIndex, bool value)
        {
            _dataSet.Booleans[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        private void ChangeFloatValue(uint typeSpecificIndex, float value)
        {
            _dataSet.Floats[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        private void ChangeIntValue(uint typeSpecificIndex, int value)
        {
            _dataSet.Integers[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        private void ChangeStringValue(uint typeSpecificIndex, string value)
        {
            _dataSet.Strings[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        private void ChangeVectorValue(uint typeSpecificIndex, Vector3 value)
        {
            _dataSet.Vectors[typeSpecificIndex] = value;
            OnValueUpdate();
        }

        #endregion
    }
}