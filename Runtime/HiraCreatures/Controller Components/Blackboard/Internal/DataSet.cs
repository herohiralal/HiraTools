using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
    public class DataSet : IReadWriteBlackboardDataSet
    {
        public DataSet(uint boolKeyCount, uint floatKeyCount, uint intKeyCount, uint stringKeyCount,
            uint vectorKeyCount) : this(boolKeyCount, floatKeyCount, intKeyCount, stringKeyCount, vectorKeyCount,
            new List<DataSet>())
        {
        }


        private DataSet(uint boolKeyCount, uint floatKeyCount, uint intKeyCount, uint stringKeyCount,
            uint vectorKeyCount, List<DataSet> pool)
        {
            Booleans = new bool[boolKeyCount];
            Floats = new float[floatKeyCount];
            Integers = new int[intKeyCount];
            Strings = new string[stringKeyCount];
            Vectors = new Vector3[vectorKeyCount];
            _pool = pool;
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

        private DataSet _parent;
        private readonly List<DataSet> _pool;

        public IReadWriteBlackboardDataSet GetPooledDuplicateWithoutCopyingData()
        {
            if (_parent != null) return _parent.GetPooledDuplicateWithoutCopyingData();
            if (_pool.Count == 0)
            {
                var copy = new DataSet((uint) Booleans.Length,
                    (uint) Floats.Length,
                    (uint) Integers.Length,
                    (uint) Strings.Length,
                    (uint) Vectors.Length,
                    null);
                _pool.Add(copy);
            }

            var lastIndex = _pool.Count - 1;
            var pooledDataSet = _pool[lastIndex];
            _pool.RemoveAt(lastIndex);
            pooledDataSet._parent = this;

            return pooledDataSet;
        }

        public IReadWriteBlackboardDataSet GetPooledDuplicate()
        {
            var pooledDataSet = GetPooledDuplicateWithoutCopyingData();
            CopyTo(pooledDataSet);
            return pooledDataSet;
        }

        public void Return(IReadWriteBlackboardDataSet readWriteDataSet)
        {
            var pool = _parent == null ? _pool : _parent._pool;
            if (!(readWriteDataSet is DataSet dataSet) || pool.Contains(dataSet)) return;
            dataSet._parent = null;
            pool.Add(dataSet);
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