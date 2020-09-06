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
        private HiraBlackboardComponent defaultBlackboard = null;

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

            if (keyType == HiraBlackboardKeyType.Undefined || keyType != keys[hash].KeyType)
                throw new InvalidCastException($"Hash mismatch with type {keyType}.");
        }

        internal bool IsInstanceSynced(uint hash) => keys[hash].InstanceSynchronized;

        internal uint GetTypeSpecificIndex(uint hash) => keys[hash].TypeSpecificIndex;

        #endregion

        private void OnEnable()
        {
            InstanceSynchronizer = new HiraBlackboardInstanceSynchronizer(this);
            (indices, defaultBlackboard) = BuildCache();
            defaultBlackboard.RequestSynchronizationWithKeySet();
        }

        private void OnDisable()
        {
            defaultBlackboard.BreakSynchronizationWithKeySet();
            defaultBlackboard = null;
            indices = null;
            InstanceSynchronizer = null;
        }

        internal HiraBlackboardComponent GetFreshBlackboardComponent() =>
            new HiraBlackboardComponent(this, defaultBlackboard.valueSet.Copy());

        #region Index Cache

        private (Dictionary<string, uint>, HiraBlackboardComponent) BuildCache()
        {
            uint boolKeyCount = 0;
            uint floatKeyCount = 0;
            uint intKeyCount = 0;
            uint stringKeyCount = 0;
            uint vectorKeyCount = 0;

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
                            keys[i].TypeSpecificIndex = boolKeyCount++;
                            break;
                        case HiraBlackboardKeyType.Float:
                            keys[i].TypeSpecificIndex = floatKeyCount++;
                            break;
                        case HiraBlackboardKeyType.Int:
                            keys[i].TypeSpecificIndex = intKeyCount++;
                            break;
                        case HiraBlackboardKeyType.String:
                            keys[i].TypeSpecificIndex = stringKeyCount++;
                            break;
                        case HiraBlackboardKeyType.Vector:
                            keys[i].TypeSpecificIndex = vectorKeyCount++;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            var blackboardComponent = new HiraBlackboardComponent(this,
                new HiraBlackboardValueSet(boolKeyCount,
                    floatKeyCount,
                    intKeyCount,
                    stringKeyCount,
                    vectorKeyCount));

            return (keyIndices, blackboardComponent);
        }

        #endregion
    }
}