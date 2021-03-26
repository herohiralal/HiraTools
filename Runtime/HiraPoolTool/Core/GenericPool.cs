using System.Collections.Generic;

namespace UnityEngine
{
    public interface IPoolable
    {
        void OnRetrieve();
        void OnReturn();
    }
    
    public static class GenericPool<T> where T : IPoolable, new()
    {
        private static readonly List<T> pool = new List<T>();
        
        public static T Retrieve()
        {
            if (pool.Count == 0)
                pool.Add(new T());

            var last = pool.Count - 1;
            var output = pool[last];
            pool.RemoveAt(last);
            output.OnRetrieve();
            return output;
        }

        public static void Return(T target)
        {
            target.OnReturn();

            pool.Add(target);
        }
    }
}