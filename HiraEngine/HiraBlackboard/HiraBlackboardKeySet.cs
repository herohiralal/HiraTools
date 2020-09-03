/*
 * Name: HiraBlackboardKeySet.cs
 * Created By: Rohan Jadav
 * Description: A set of keys for a blackboard.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hiralal.Blackboard
{
    [CreateAssetMenu]
    public class HiraBlackboardKeySet : ScriptableObject
    {
        [SerializeField] private HiraBlackboardKey[] keys = null;
        private Dictionary<string, uint> keyIndices = null;
        private Dictionary<string, bool> instanceSyncMap = null;
        internal HiraMap MapComponent { get; private set; } = null;
        

        private uint boolKeyCount = 0;
        private uint floatKeyCount = 0;
        private uint integerKeyCount = 0;
        private uint stringKeyCount = 0;
        private uint vectorKeyCount = 0;
        private uint unityObjectKeyCount = 0;

        internal uint BoolKeyCount => boolKeyCount;
        internal uint FloatKeyCount => floatKeyCount;
        internal uint IntegerKeyCount => integerKeyCount;
        internal uint StringKeyCount => stringKeyCount;
        internal uint VectorKeyCount => vectorKeyCount;
        internal uint UnityObjectKeyCount => unityObjectKeyCount;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            keyIndices.Clear();
            keyIndices = null;
            
            instanceSyncMap.Clear();
            instanceSyncMap = null;
            
            MapComponent = null;
        }

        internal uint GetIndex(in string key) => keyIndices[key];
        internal bool IsInstanceSynced(in string key) => instanceSyncMap[key];
        
        #region Initialization

        private void Initialize()
        {
            boolKeyCount = 0;
            floatKeyCount = 0;
            integerKeyCount = 0;
            stringKeyCount = 0;
            vectorKeyCount = 0;
            unityObjectKeyCount = 0;


            uint boolKeyCountInstanceSynced = 0;
            uint floatKeyCountInstanceSynced = 0;
            uint integerKeyCountInstanceSynced = 0;
            uint stringKeyCountInstanceSynced = 0;
            uint vectorKeyCountInstanceSynced = 0;
            uint unityObjectKeyCountInstanceSynced = 0;

            keyIndices = new Dictionary<string, uint>();
            instanceSyncMap = new Dictionary<string, bool>();

            foreach (var key in keys)
            {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (key.KeyType)
                {
                    case HiraBlackboardKeyType.Bool:
                        AddKey(key, ref boolKeyCount, ref boolKeyCountInstanceSynced);
                        break;
                    case HiraBlackboardKeyType.Float:
                        AddKey(key, ref floatKeyCount, ref floatKeyCountInstanceSynced);
                        break;
                    case HiraBlackboardKeyType.Int:
                        AddKey(key, ref integerKeyCount, ref integerKeyCountInstanceSynced);
                        break;
                    case HiraBlackboardKeyType.String:
                        AddKey(key, ref stringKeyCount, ref stringKeyCountInstanceSynced);
                        break;
                    case HiraBlackboardKeyType.Vector:
                        AddKey(key, ref vectorKeyCount, ref vectorKeyCountInstanceSynced);
                        break;
                    case HiraBlackboardKeyType.UnityObject:
                        AddKey(key, ref unityObjectKeyCount, ref unityObjectKeyCountInstanceSynced);
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            MapComponent = new HiraMap(this, boolKeyCountInstanceSynced,
                floatKeyCountInstanceSynced,
                integerKeyCountInstanceSynced,
                stringKeyCountInstanceSynced,
                vectorKeyCountInstanceSynced,
                unityObjectKeyCountInstanceSynced);
        }

        private void AddKey(HiraBlackboardKey key, ref uint index, ref uint instanceSyncedIndex)
        {
            if (!keyIndices.ContainsKey(key.Name))
            {
                instanceSyncMap.Add(key.Name, key.InstanceSynchronized);
                if (key.InstanceSynchronized)
                {
                    keyIndices.Add(key.Name, instanceSyncedIndex);
                    instanceSyncedIndex++;
                }
                else
                {
                    keyIndices.Add(key.Name, index);
                    index++;
                }
            }
            else Debug.LogErrorFormat(this, $"Duplicate key {key.Name} in HiraBlackboardKeySet {name}.");
        }
        
        #endregion
    }
}