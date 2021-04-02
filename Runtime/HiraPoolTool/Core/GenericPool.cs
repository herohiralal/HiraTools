using System.Collections.Generic;

namespace UnityEngine
{
    public interface IPoolRetrievalCallbackReceiver
    {
        void OnRetrieve();
    }

    public interface IPoolReturnCallbackReceiver
    {
        void OnReturn();
    }

    public static class GenericPool<T> where T : new()
    {
        private static readonly List<T> pool = new List<T>();

        public static T Retrieve()
        {
            if (pool.Count == 0)
                pool.Add(new T());

            var last = pool.Count - 1;
            var output = pool[last];
            pool.RemoveAt(last);
            if (output is IPoolRetrievalCallbackReceiver pooledObject)
                pooledObject.OnRetrieve();
            return output;
        }

        public static void Return(T target)
        {
            if (target is IPoolReturnCallbackReceiver pooledObject)
                pooledObject.OnReturn();

            pool.Add(target);
        }

        public static void Empty() => pool.Clear();
    }
}