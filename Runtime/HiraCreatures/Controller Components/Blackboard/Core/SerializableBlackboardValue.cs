using System;
using HiraEngine.Components.Blackboard;

namespace UnityEngine
{
    [Serializable]
    public class SerializableBlackboardValue : IBlackboardValueConstructorParams
    {
        [SerializeField] private HiraBlackboardKeySet keySet = null;

        public HiraBlackboardKeySet KeySet
        {
            get => keySet;
            set => keySet = value;
        }

        [SerializeField] private SerializableBlackboardKey key = null;

        [SerializeField] private string typeString = null;

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

        public IBlackboardValue Value => BlackboardTypes.GetValue(typeString, this);
    }
}