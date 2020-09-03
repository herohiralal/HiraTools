/*
 * Name: HiraMap.cs
 * Created By: Rohan Jadav
 * Description: Stores the values for a blackboard.
 */

using UnityEngine;

namespace Hiralal.Blackboard
{
    internal class HiraMap : IHiraMap
    {
        internal HiraMap(HiraBlackboardKeySet keySet,
            uint boolValuesCount,
            uint floatValuesCount,
            uint integerValuesCount,
            uint stringValuesCount,
            uint vectorValuesCount,
            uint unityObjectValuesCount)
        {
            blackboard = keySet;
            booleanValues = new bool[boolValuesCount];
            floatValues = new float[floatValuesCount];
            integerValues = new int[integerValuesCount];
            stringValues = new string[stringValuesCount];
            vectorValues = new Vector3[vectorValuesCount];
            unityObjectValues = new Object[unityObjectValuesCount];
        }

        internal HiraMap(HiraBlackboardKeySet keySet) : this(keySet, 
            keySet.BoolKeyCount,
            keySet.FloatKeyCount,
            keySet.IntegerKeyCount,
            keySet.StringKeyCount,
            keySet.VectorKeyCount,
            keySet.UnityObjectKeyCount)
        {
            
        }

        private readonly HiraBlackboardKeySet blackboard;
        private readonly bool[] booleanValues;
        private readonly float[] floatValues;
        private readonly int[] integerValues;
        private readonly string[] stringValues;
        private readonly Vector3[] vectorValues;
        private readonly Object[] unityObjectValues;

        private HiraMap GetMap(in string key) => blackboard.IsInstanceSynced(key) ? blackboard.MapComponent : this;

        // Boolean
        public bool GetValueAsBool(in string key) => GetMap(in key).booleanValues[blackboard.GetIndex(in key)];
        public void SetValueAsBool(in string key, in bool value) => GetMap(in key).booleanValues[blackboard.GetIndex(in key)] = value;

        // Float
        public float GetValueAsFloat(in string key) => GetMap(in key).floatValues[blackboard.GetIndex(in key)];
        public void SetValueAsFloat(in string key, in float value) => GetMap(in key).floatValues[blackboard.GetIndex(in key)] = value;

        // Integer
        public int GetValueAsInteger(in string key) => GetMap(in key).integerValues[blackboard.GetIndex(in key)];
        public void SetValueAsInteger(in string key, in int value) => GetMap(in key).integerValues[blackboard.GetIndex(in key)] = value;

        // String
        public string GetValueAsString(in string key) => GetMap(in key).stringValues[blackboard.GetIndex(in key)];
        public void SetValueAsString(in string key, in string value) => GetMap(in key).stringValues[blackboard.GetIndex(in key)] = value;

        // Vector
        public Vector3 GetValueAsVector3(in string key) => GetMap(in key).vectorValues[blackboard.GetIndex(in key)];
        public void SetValueAsVector3(in string key, in Vector3 value) => GetMap(in key).vectorValues[blackboard.GetIndex(in key)] = value;

        // Object
        public Object GetValueAsObject(in string key) => GetMap(in key).unityObjectValues[blackboard.GetIndex(in key)];
        public void SetValueAsObject(in string key, in Object value) => GetMap(in key).unityObjectValues[blackboard.GetIndex(in key)] = value;
    }
}