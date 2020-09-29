using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public abstract class HiraCollection<T> : ScriptableObject, IEnumerable<T>
        where T : ScriptableObject
    {
        [SerializeField] protected T[] collection = null;
        public Type CollectionType => typeof(T);

        public void Setup(T[] inCollection) => collection = inCollection;

        public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}