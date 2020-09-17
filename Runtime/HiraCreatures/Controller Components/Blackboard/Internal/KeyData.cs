using System;
using System.Collections.Generic;
using HiraCreatures.Components.Blackboard.Helpers;
using UnityEngine;

namespace HiraCreatures.Components.Blackboard.Internal
{
    public class KeyData : IBlackboardKeyData
    {
        public KeyData(SerializableKey[] keys) => _keys = keys;
        
        private readonly SerializableKey[] _keys;
        private Dictionary<string, uint> _hashes = null;

        public IReadOnlyInstanceSynchronizer InstanceSynchronizer { get; private set; }

        public IBlackboardValueAccessor ValueAccessor { get; private set; }

        public void Activate()
        {
            IBlackboardDataSet dataSet;
            (dataSet, _hashes) = BuildCache();
            var instanceSynchronizer = BlackboardTypes.GetSynchronizer();
            InstanceSynchronizer = instanceSynchronizer;
            ValueAccessor = BlackboardTypes.GetKeySetValueAccessor(this, dataSet, instanceSynchronizer);
        }

        public void Deactivate()
        {
            ValueAccessor = null;
            InstanceSynchronizer = null;
            _hashes = null;
        }

        #region Key Data

        public uint GetHash(in string keyName) =>
            _hashes[keyName];

        public uint GetTypeSpecificIndex(uint hash) =>
            _keys[hash].TypeSpecificIndex;

        public bool IsInstanceSynchronized(uint hash) =>
            _keys[hash].InstanceSynchronized;

        #endregion

        #region Cache-Building

        private (IBlackboardDataSet, Dictionary<string, uint>) BuildCache()
        {
            uint booleans = 0, floats = 0, ints = 0, strings = 0, vectors = 0;
            var hashes = new Dictionary<string, uint>();

            for (uint i = 0; i < _keys.Length; i++)
            {
                if (hashes.ContainsKey(_keys[i].Name))
                    Debug.LogErrorFormat($"Duplicate key \"{_keys[i].Name}\".");
                else
                {
                    hashes.Add(_keys[i].Name, i);

                    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                    switch (_keys[i].KeyType)
                    {
                        case BlackboardKeyType.Bool:
                            _keys[i].TypeSpecificIndex = booleans++;
                            break;
                        case BlackboardKeyType.Float:
                            _keys[i].TypeSpecificIndex = floats++;
                            break;
                        case BlackboardKeyType.Int:
                            _keys[i].TypeSpecificIndex = ints++;
                            break;
                        case BlackboardKeyType.String:
                            _keys[i].TypeSpecificIndex = strings++;
                            break;
                        case BlackboardKeyType.Vector:
                            _keys[i].TypeSpecificIndex = vectors++;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            var dataSet = BlackboardTypes.GetDataSet(booleans, floats, ints, 
                strings, vectors);
            return (dataSet, hashes);
        }

        #endregion
    }
}