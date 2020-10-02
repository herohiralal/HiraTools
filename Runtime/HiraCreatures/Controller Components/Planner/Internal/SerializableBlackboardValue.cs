using System;
using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    [Serializable]
    public abstract class SerializableBlackboardValue : IBlackboardValueConstructorParams
    {
        public void Setup<T>(HiraBlackboardKeySet inKeySet,
            SerializableBlackboardKey inKey,
            BoolReference inBoolValue = default,
            FloatReference inFloatValue = default,
            IntReference inIntValue = default,
            StringReference inStringValue = default,
            Vector3Reference inVectorValue = default)
        {
            keySet = inKeySet;
            key = inKey;
            boolValue = inBoolValue;
            floatValue = inFloatValue;
            intValue = inIntValue;
            stringValue = inStringValue;
            vectorValue = inVectorValue;
            typeString = typeof(T).GetReflectionName();
        }

        [SerializeField] private HiraBlackboardKeySet keySet = null;

        public HiraBlackboardKeySet KeySet
        {
            get => keySet;
            set => keySet = value;
        }

        [SerializeField] private SerializableBlackboardKey key = null;

        [SerializeField] protected string typeString = null;

        [SerializeField] private BoolReference boolValue = null;
        [SerializeField] private FloatReference floatValue = null;
        [SerializeField] private IntReference intValue = null;
        [SerializeField] private StringReference stringValue = null;
        [SerializeField] private Vector3Reference vectorValue = null;
        
        public uint TypeSpecificIndex => keySet.GetTypeSpecificIndex(keySet.GetHash(key.Name));
        public bool BoolValue => boolValue.Value;
        public float FloatValue => floatValue.Value;
        public int IntValue => intValue.Value;
        public string StringValue => stringValue.Value;
        public Vector3 VectorValue => vectorValue.Value;
    }
}