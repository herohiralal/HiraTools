using System;

namespace UnityEngine
{
    public interface IHiraControllerBlackboard : IHiraControllerComponent, IBlackboardValueAccessor
    {
    }

    public interface IBlackboardValueAccessor
    {
        IReadOnlyBlackboardDataSet DataSet { get; }
        event Action OnValueUpdate;
        uint GetHash(in string keyName);
        bool GetBooleanValue(uint hash);
        float GetFloatValue(uint hash);
        int GetIntValue(uint hash);
        string GetStringValue(uint hash);
        Vector3 GetVectorValue(uint hash);
        void SetBooleanValue(uint hash, bool value);
        void SetFloatValue(uint hash, float value);
        void SetIntValue(uint hash, int value);
        void SetStringValue(uint hash, string value);
        void SetVectorValue(uint hash, Vector3 value);
    }

    public interface IReadOnlyBlackboardDataSet
    {
        bool GetBoolean(uint index);
        float GetFloat(uint index);
        int GetInteger(uint index);
        string GetString(uint index);
        Vector3 GetVector(uint index);
        IReadWriteBlackboardDataSet GetDuplicate();
        void CopyTo(IReadWriteBlackboardDataSet duplicate);
    }

    public interface IReadWriteBlackboardDataSet : IReadOnlyBlackboardDataSet
    {
        bool[] Booleans { get; }
        float[] Floats { get; }
        int[] Integers { get; }
        string[] Strings { get; }
        Vector3[] Vectors { get; }
    }
}