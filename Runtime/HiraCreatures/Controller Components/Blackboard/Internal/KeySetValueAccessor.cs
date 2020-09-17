using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace HiraCreatures.Components.Blackboard.Internal
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

        public uint GetHash(in string keyName) => _keyData.GetHash(keyName);

        public bool GetBooleanValue(uint hash)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            return _dataSet.Booleans[_keyData.GetTypeSpecificIndex(hash)];
        }

        public float GetFloatValue(uint hash)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            return _dataSet.Floats[_keyData.GetTypeSpecificIndex(hash)];
        }

        public int GetIntValue(uint hash)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            return _dataSet.Integers[_keyData.GetTypeSpecificIndex(hash)];
        }

        public string GetStringValue(uint hash)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            return _dataSet.Strings[_keyData.GetTypeSpecificIndex(hash)];
        }

        public Vector3 GetVectorValue(uint hash)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            return _dataSet.Vectors[_keyData.GetTypeSpecificIndex(hash)];
        }

        public void SetBooleanValue(uint hash, bool value)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            var typeSpecificIndex = _keyData.GetTypeSpecificIndex(hash);
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_boolean(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Booleans[typeSpecificIndex] = value;
        }

        public void SetFloatValue(uint hash, float value)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            var typeSpecificIndex = _keyData.GetTypeSpecificIndex(hash);
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_float(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Floats[typeSpecificIndex] = value;
        }

        public void SetIntValue(uint hash, int value)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            var typeSpecificIndex = _keyData.GetTypeSpecificIndex(hash);
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_integer(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Integers[typeSpecificIndex] = value;
        }

        public void SetStringValue(uint hash, string value)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            var typeSpecificIndex = _keyData.GetTypeSpecificIndex(hash);
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_string(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Strings[typeSpecificIndex] = value;
        }

        public void SetVectorValue(uint hash, Vector3 value)
        {
            Assert.IsTrue(_keyData.IsInstanceSynchronized(hash));
            var typeSpecificIndex = _keyData.GetTypeSpecificIndex(hash);
            _instanceSynchronizer.ReportSyncedInstanceValueUpdate_vector(typeSpecificIndex, value);
            OnValueUpdate();
            _dataSet.Vectors[typeSpecificIndex] = value;
        }
    }
}