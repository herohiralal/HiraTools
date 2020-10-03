using HiraEngine.Components.Planner;
using UnityEngine;

namespace HiraTests.HiraEngine.Components.Planner
{
    public class BlackboardValueConstructorParamsMock : IBlackboardValueConstructorParams
    {
        public BlackboardValueConstructorParamsMock(uint typeSpecificIndex = default,
            bool boolValue = default,
            float floatValue = default,
            int intValue = default,
            string stringValue = default,
            Vector3 vectorValue = default)
        {
            TypeSpecificIndex = typeSpecificIndex;
            BoolValue = boolValue;
            FloatValue = floatValue;
            IntValue = intValue;
            StringValue = stringValue;
            VectorValue = vectorValue;
        }

        public uint TypeSpecificIndex { get; }
        public bool BoolValue { get; }
        public float FloatValue { get; }
        public int IntValue { get; }
        public string StringValue { get; }
        public Vector3 VectorValue { get; }
    }
}