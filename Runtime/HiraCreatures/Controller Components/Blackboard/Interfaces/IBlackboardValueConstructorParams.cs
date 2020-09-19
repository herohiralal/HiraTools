using UnityEngine;

namespace HiraEngine.Components.Blackboard
{
    public interface IBlackboardValueConstructorParams
    {
        uint TypeSpecificIndex { get; }
        bool BoolValue { get; }
        float FloatValue { get; }
        int IntValue { get; }
        string StringValue { get; }
        Vector3 VectorValue { get; }
    }
}