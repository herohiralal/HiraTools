using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hiralal.Blackboard
{
    [CreateAssetMenu(fileName = "New HiraBlackboard Key Set", menuName = "Hiralal/HiraEngine/HiraBlackboard/Key Set")]
    public sealed class HiraBlackboardKeySet : ScriptableObject
    {
        [SerializeField] private HiraBlackboardKey[] keys = null;
        private Dictionary<string, uint> indices = null;

        public uint BooleanKeyCount { get; private set; } = 0;
        public uint FloatKeyCount { get; private set; } = 0;
        public uint IntegerKeyCount { get; private set; } = 0;
        public uint StringKeyCount { get; private set; } = 0;
        public uint VectorKeyCount { get; private set; } = 0;

        internal HiraBlackboardInstanceSynchronizer InstanceSynchronizer { get; private set; }

        #region Key-Specific Queries

        public uint GetHash(in string keyName)
        {
            if (indices.ContainsKey(keyName)) return indices[keyName];
            throw new NullReferenceException($"HiraBlackboardKeySet \"{name}\" could not" +
                                             $" find a key with the name \"{keyName}\".");
        }

        public void ValidateTransaction(uint hash, HiraBlackboardKeyType keyType)
        {
            if (hash >= keys.Length)
                throw new NullReferenceException("Invalid hash.");
            
            if(keyType==HiraBlackboardKeyType.Undefined || keyType!= keys[hash].KeyType)
                throw new InvalidCastException($"Hash mismatch with type {keyType}.");
        }

        internal bool IsInstanceSynced(uint hash) => keys[hash].InstanceSynchronized;

        internal uint GetTypeSpecificIndex(uint hash) => keys[hash].TypeSpecificIndex;
        
        #endregion

        private void OnEnable()
        {
            InstanceSynchronizer = new HiraBlackboardInstanceSynchronizer(this);
            indices = BuildIndexCache();
        }

        private void OnDisable()
        {
            indices = null;
            InstanceSynchronizer = null;
        }

        #region Index Cache

        private Dictionary<string, uint> BuildIndexCache()
        {
            BooleanKeyCount = 0;
            FloatKeyCount = 0;
            IntegerKeyCount = 0;
            StringKeyCount = 0;
            VectorKeyCount = 0;
            
            var keyIndices = new Dictionary<string, uint>();

            for (uint i = 0; i < keys.Length; i++)
            {
                if (keyIndices.ContainsKey(keys[i].Name))
                    Debug.LogErrorFormat(this, $"Duplicate key \"{keys[i].Name}\" in HiraBlackboardKeySet \"{name}\".");
                else
                {
                    keyIndices.Add(keys[i].Name, i);

                    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                    switch (keys[i].KeyType)
                    {
                        case HiraBlackboardKeyType.Bool:
                            keys[i].TypeSpecificIndex = BooleanKeyCount++;
                            break;
                        case HiraBlackboardKeyType.Float:
                            keys[i].TypeSpecificIndex = FloatKeyCount++;
                            break;
                        case HiraBlackboardKeyType.Int:
                            keys[i].TypeSpecificIndex = IntegerKeyCount++;
                            break;
                        case HiraBlackboardKeyType.String:
                            keys[i].TypeSpecificIndex = StringKeyCount++;
                            break;
                        case HiraBlackboardKeyType.Vector:
                            keys[i].TypeSpecificIndex = VectorKeyCount++;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return keyIndices;
        }

        #endregion
    }
}