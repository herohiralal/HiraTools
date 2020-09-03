/*
 * Name: IHiraMap.cs
 * Created By: Rohan Jadav
 * Description: Interface for a HiraMap.
 */

using UnityEngine;

namespace Hiralal.Blackboard
{
    public interface IHiraMap
    {
        // Boolean
        bool GetValueAsBool(in string key);
        void SetValueAsBool(in string key, in bool value);
        
        // Float
        float GetValueAsFloat(in string key);
        void SetValueAsFloat(in string key, in float value);
        
        // Integer
        int GetValueAsInteger(in string key);
        void SetValueAsInteger(in string key, in int value);
        
        // String
        string GetValueAsString(in string key);
        void SetValueAsString(in string key, in string value);
        
        // Vector3
        Vector3 GetValueAsVector3(in string key);
        void SetValueAsVector3(in string key, in Vector3 value);
        
        // Object
        Object GetValueAsObject(in string key);
        void SetValueAsObject(in string key, in Object value);
    }
}