using UnityEngine;

namespace Hiralal.Blackboard
{
    internal class HiraBlackboardValues
    {
        private readonly HiraBlackboardKeySet keySet = null;

        internal readonly bool[] booleans = null;
        internal readonly float[] floats = null;
        internal readonly int[] integers = null;
        internal readonly string[] strings = null;
        internal readonly Vector3[] vectors = null;

        internal HiraBlackboardValues(HiraBlackboardKeySet keySet)
        {
            this.keySet = keySet;
            booleans = new bool[keySet.BooleanKeyCount];
            floats = new float[keySet.FloatKeyCount];
            integers = new int[keySet.IntegerKeyCount];
            strings = new string[keySet.StringKeyCount];
            vectors = new Vector3[keySet.VectorKeyCount];
        }

        private bool synced = false;

        internal void RequestSynchronizationWithKeySet()
        {
            if (synced) return;
            synced = true;

            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateBoolean += OnSyncInstanceValueUpdate_boolean;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateFloat += OnSyncInstanceValueUpdate_float;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateInteger += OnSyncInstanceValueUpdate_integer;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateString += OnSyncInstanceValueUpdate_string;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateVector += OnSyncInstanceValueUpdate_vector;
        }

        internal void BreakSynchronizationWithKeySet()
        {
            if (!synced) return;
            synced = false;

            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateBoolean -= OnSyncInstanceValueUpdate_boolean;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateFloat -= OnSyncInstanceValueUpdate_float;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateInteger -= OnSyncInstanceValueUpdate_integer;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateString -= OnSyncInstanceValueUpdate_string;
            keySet.InstanceSynchronizer.OnSyncInstanceValueUpdateVector -= OnSyncInstanceValueUpdate_vector;
        }

        internal HiraBlackboardValues Copy()
        {
            var copy = new HiraBlackboardValues(keySet);

            for (var i = 0; i < booleans.Length; i++) copy.booleans[i] = booleans[i];
            for (var i = 0; i < floats.Length; i++) copy.floats[i] = floats[i];
            for (var i = 0; i < integers.Length; i++) copy.integers[i] = integers[i];
            for (var i = 0; i < strings.Length; i++) copy.strings[i] = strings[i];
            for (var i = 0; i < vectors.Length; i++) copy.vectors[i] = vectors[i];

            return copy;
        }

        private void OnSyncInstanceValueUpdate_boolean(uint typeSpecificIndex, bool newValue) =>
            booleans[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_float(uint typeSpecificIndex, float newValue) =>
            floats[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_integer(uint typeSpecificIndex, int newValue) =>
            integers[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_string(uint typeSpecificIndex, string newValue) =>
            strings[typeSpecificIndex] = newValue;

        private void OnSyncInstanceValueUpdate_vector(uint typeSpecificIndex, Vector3 newValue) =>
            vectors[typeSpecificIndex] = newValue;
    }
}