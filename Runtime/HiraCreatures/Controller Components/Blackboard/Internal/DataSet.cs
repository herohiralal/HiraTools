using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
    public class DataSet : IReadWriteBlackboardDataSet
    {
        public DataSet(uint boolKeyCount, uint floatKeyCount, uint intKeyCount, uint stringKeyCount, uint vectorKeyCount)
        {
            Booleans = new bool[boolKeyCount];
            Floats = new float[floatKeyCount];
            Integers = new int[intKeyCount];
            Strings = new string[stringKeyCount];
            Vectors = new Vector3[vectorKeyCount];
        }
        
        public bool[] Booleans { get; }
        public float[] Floats { get; }
        public int[] Integers { get; }
        public string[] Strings { get; }
        public Vector3[] Vectors { get; }

        public bool GetBoolean(uint index) => Booleans[index];
        public float GetFloat(uint index) => Floats[index];
        public int GetInteger(uint index) => Integers[index];
        public string GetString(uint index) => Strings[index];
        public Vector3 GetVector(uint index) => Vectors[index];

        public IReadWriteBlackboardDataSet GetDuplicate()
        {
            var copy = new DataSet((uint) Booleans.Length,
                (uint) Floats.Length,
                (uint) Integers.Length,
                (uint) Strings.Length,
                (uint) Vectors.Length);

            CopyTo(copy);

            return copy;
        }

        public void CopyTo(IReadWriteBlackboardDataSet duplicate)
        {
            for (var i = 0; i < Booleans.Length; i++) duplicate.Booleans[i] = Booleans[i];
            for (var i = 0; i < Floats.Length; i++) duplicate.Floats[i] = Floats[i];
            for (var i = 0; i < Integers.Length; i++) duplicate.Integers[i] = Integers[i];
            for (var i = 0; i < Strings.Length; i++) duplicate.Strings[i] = Strings[i];
            for (var i = 0; i < Vectors.Length; i++) duplicate.Vectors[i] = Vectors[i];
        }
    }
}