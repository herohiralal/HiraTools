namespace UnityEngine
{
    public static class DatasetHelper
    {
        public static void Reset(this IReadWriteBlackboardDataSet dataSet)
        {
            dataSet.Booleans.ResetArray(false);
            dataSet.Floats.ResetArray(0f);
            dataSet.Integers.ResetArray(0);
            dataSet.Strings.ResetArray("");
            dataSet.Vectors.ResetArray(Vector3.zero);
        }

        private static void ResetArray<T>(this T[] array, T value)
        {
            var count = array.Length;
            for (var i = 0; i < count; i++) array[i] = value;
        }
    }
}