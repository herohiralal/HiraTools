namespace UnityEngine
{
    public interface IHiraBlackboard
    {
        bool GetValueAsBool(in string keyName);
        bool GetValueAsBool(uint hash);
        void SetValueAsBool(in string keyName, bool value);
        void SetValueAsBool(uint hash, bool value);
        
        float GetValueAsFloat(in string keyName);
        float GetValueAsFloat(uint hash);
        void SetValueAsFloat(in string keyName, float value);
        void SetValueAsFloat(uint hash, float value);
        
        int GetValueAsInt(in string keyName);
        int GetValueAsInt(uint hash);
        void SetValueAsInt(in string keyName, int value);
        void SetValueAsInt(uint hash, int value);
        
        string GetValueAsString(in string keyName);
        string GetValueAsString(uint hash);
        void SetValueAsString(in string keyName, string value);
        void SetValueAsString(uint hash, string value);
        
        Vector3 GetValueAsVector(in string keyName);
        Vector3 GetValueAsVector(uint hash);
        void SetValueAsVector(in string keyName, Vector3 value);
        void SetValueAsVector(uint hash, Vector3 value);
    }
}