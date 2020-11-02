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
        void ResetValues();
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
        bool GetBooleanValueWithTypeSpecificIndex(uint typeSpecificIndex);
        float GetFloatValueWithTypeSpecificIndex(uint typeSpecificIndex);
        int GetIntValueWithTypeSpecificIndex(uint typeSpecificIndex);
        string GetStringValueWithTypeSpecificIndex(uint typeSpecificIndex);
        Vector3 GetVectorValueWithTypeSpecificIndex(uint typeSpecificIndex);
        void SetBooleanValueWithTypeSpecificIndex(uint typeSpecificIndex, bool value);
        void SetFloatValueWithTypeSpecificIndex(uint typeSpecificIndex, float value);
        void SetIntValueWithTypeSpecificIndex(uint typeSpecificIndex, int value);
        void SetStringValueWithTypeSpecificIndex(uint typeSpecificIndex, string value);
        void SetVectorValueWithTypeSpecificIndex(uint typeSpecificIndex, Vector3 value);
    }

    public interface IReadOnlyBlackboardDataSet
    {
        bool GetBoolean(uint index);
        float GetFloat(uint index);
        int GetInteger(uint index);
        string GetString(uint index);
        Vector3 GetVector(uint index);
        IReadWriteBlackboardDataSet GetPooledDuplicateWithoutCopyingData();
        IReadWriteBlackboardDataSet GetPooledDuplicate();
        void CopyTo(IReadWriteBlackboardDataSet duplicate);
        void Return(IReadWriteBlackboardDataSet readWriteDataSet);
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