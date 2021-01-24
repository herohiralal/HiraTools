using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
    public class KeyData : IBlackboardKeyData
    {
        public KeyData(SerializableBlackboardKey[] keys)
        {
            _keys = keys;
            IReadWriteBlackboardDataSet dataSet;
            (dataSet, _hashes) = BuildCache(out _booleanKeys, out _floatKeys, out _intKeys, out _stringKeys, out _vectorKeys);
            var instanceSynchronizer = BlackboardTypes.GetSynchronizer();
            InstanceSynchronizer = instanceSynchronizer;
            ValueAccessor = BlackboardTypes.GetKeySetValueAccessor(this, dataSet, instanceSynchronizer);
        }

        ~KeyData()
        {
            ValueAccessor = null;
            InstanceSynchronizer = null;
            _hashes = null;
        }

        private readonly SerializableBlackboardKey[] _keys;
        private readonly SerializableBlackboardKey[] _booleanKeys;
        private readonly SerializableBlackboardKey[] _floatKeys;
        private readonly SerializableBlackboardKey[] _intKeys;
        private readonly SerializableBlackboardKey[] _stringKeys;
        private readonly SerializableBlackboardKey[] _vectorKeys;
        private Dictionary<string, uint> _hashes = null;

        public IReadOnlyInstanceSynchronizer InstanceSynchronizer { get; private set; }

        public IBlackboardValueAccessor ValueAccessor { get; private set; }

        #region Key Data

        public uint GetHash(in string keyName) =>
            _hashes[keyName];

        public uint GetTypeSpecificIndex(uint hash) =>
            _keys[hash].TypeSpecificIndex;

        public bool IsInstanceSynchronized(uint hash) =>
            _keys[hash].InstanceSynchronized;

        public bool IsBooleanKeyInstanceSynchronized(uint typeSpecificIndex) =>
            _booleanKeys[typeSpecificIndex].InstanceSynchronized;

        public bool IsFloatKeyInstanceSynchronized(uint typeSpecificIndex) =>
            _floatKeys[typeSpecificIndex].InstanceSynchronized;

        public bool IsIntKeyInstanceSynchronized(uint typeSpecificIndex) =>
            _intKeys[typeSpecificIndex].InstanceSynchronized;

        public bool IsStringKeyInstanceSynchronized(uint typeSpecificIndex) =>
            _stringKeys[typeSpecificIndex].InstanceSynchronized;

        public bool IsVectorKeyInstanceSynchronized(uint typeSpecificIndex) =>
            _vectorKeys[typeSpecificIndex].InstanceSynchronized;

        #endregion

        #region Cache-Building

        private (IReadWriteBlackboardDataSet, Dictionary<string, uint>) BuildCache(
            out SerializableBlackboardKey[] booleanKeysArray,
            out SerializableBlackboardKey[] floatKeysArray,
            out SerializableBlackboardKey[] intKeysArray,
            out SerializableBlackboardKey[] stringKeysArray,
            out SerializableBlackboardKey[] vectorKeysArray)
        {
            uint booleans = 0, floats = 0, ints = 0, strings = 0, vectors = 0;
            var hashes = new Dictionary<string, uint>();

            List<SerializableBlackboardKey> booleanKeys = new List<SerializableBlackboardKey>(),
                floatKeys = new List<SerializableBlackboardKey>(),
                intKeys = new List<SerializableBlackboardKey>(),
                stringKeys = new List<SerializableBlackboardKey>(),
                vectorKeys = new List<SerializableBlackboardKey>();


            for (uint i = 0; i < _keys.Length; i++)
            {
                var currentKey = _keys[i];
                if (hashes.ContainsKey(currentKey.Name))
                    Debug.LogErrorFormat($"Duplicate key \"{currentKey.Name}\".");
                else
                {
                    hashes.Add(currentKey.Name, i);

                    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                    switch (currentKey.KeyType)
                    {
                        case BlackboardKeyType.Bool:
                            currentKey.TypeSpecificIndex = booleans++;
                            booleanKeys.Add(currentKey);
                            break;
                        case BlackboardKeyType.Float:
                            currentKey.TypeSpecificIndex = floats++;
                            floatKeys.Add(currentKey);
                            break;
                        case BlackboardKeyType.Int:
                            currentKey.TypeSpecificIndex = ints++;
                            intKeys.Add(currentKey);
                            break;
                        case BlackboardKeyType.String:
                            currentKey.TypeSpecificIndex = strings++;
                            stringKeys.Add(currentKey);
                            break;
                        case BlackboardKeyType.Vector:
                            currentKey.TypeSpecificIndex = vectors++;
                            vectorKeys.Add(currentKey);
                            break;
                    }
                }
            }

            (booleanKeysArray, floatKeysArray, intKeysArray, stringKeysArray, vectorKeysArray) =
                (booleanKeys.ToArray(), floatKeys.ToArray(), intKeys.ToArray(), stringKeys.ToArray(), vectorKeys.ToArray());

            var dataSet = BlackboardTypes.GetWriteableDataSet(booleans, floats, ints,
                strings, vectors);
            return (dataSet, hashes);
        }

        #endregion
    }
}