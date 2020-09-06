using UnityEngine;
using System;

namespace Hiralal.Blackboard
{
    [Serializable]
    public class HiraBlackboardKey
    {
        [SerializeField] private StringReference name = null;
        internal string Name => name.Value;
        
        [SerializeField] private HiraBlackboardKeyType keyType = HiraBlackboardKeyType.Undefined;
        internal HiraBlackboardKeyType KeyType => keyType;
        
        [SerializeField] private bool instanceSynchronized = false;
        internal bool InstanceSynchronized => instanceSynchronized;

        internal uint TypeSpecificIndex { get; set; } = 0;
    }
}