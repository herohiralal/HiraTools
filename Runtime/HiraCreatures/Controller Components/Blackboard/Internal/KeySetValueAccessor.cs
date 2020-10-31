using System;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
    public class KeySetValueAccessor : IBlackboardValueAccessor
    {
        public KeySetValueAccessor(IBlackboardKeyData keyData,
            IReadWriteBlackboardDataSet dataSet,
            IInstanceSynchronizer instanceSynchronizer) =>
            (_keyData, _dataSet, _instanceSynchronizer) = (keyData, dataSet, instanceSynchronizer);

        private readonly IBlackboardKeyData _keyData;
        private readonly IReadWriteBlackboardDataSet _dataSet;
        private readonly IInstanceSynchronizer _instanceSynchronizer;

        public IReadOnlyBlackboardDataSet DataSet => _dataSet;
        public event Action OnValueUpdate = delegate { };

        public void Reset() => _dataSet.Reset();

        public uint GetHash(in string keyName) => _keyData.GetHash(keyName);

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

        public void SetBooleanValue(uint hash, bool value) => 
            SetBooleanValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash), value);

        public void SetBooleanValueWithTypeSpecificIndex(uint typeSpecificIndex, bool value)
        {
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_boolean(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Booleans[typeSpecificIndex] = value;
        }

        public void SetFloatValue(uint hash, float value) => 
            SetFloatValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash), value);

        public void SetFloatValueWithTypeSpecificIndex(uint typeSpecificIndex, float value)
        {
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_float(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Floats[typeSpecificIndex] = value;
        }

        public void SetIntValue(uint hash, int value) => 
            SetIntValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash), value);

        public void SetIntValueWithTypeSpecificIndex(uint typeSpecificIndex, int value)
        {
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_integer(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Integers[typeSpecificIndex] = value;
        }

        public void SetStringValue(uint hash, string value) => 
            SetStringValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash), value);

        public void SetStringValueWithTypeSpecificIndex(uint typeSpecificIndex, string value)
        {
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_string(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Strings[typeSpecificIndex] = value;
        }

        public void SetVectorValue(uint hash, Vector3 value) => 
            SetVectorValueWithTypeSpecificIndex(_keyData.GetTypeSpecificIndex(hash), value);

        public void SetVectorValueWithTypeSpecificIndex(uint typeSpecificIndex, Vector3 value)
        {
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_vector(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Vectors[typeSpecificIndex] = value;
        }
    }
}