using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public interface ICollectionAwareTarget<T> where T : ScriptableObject
    {
        HiraCollection<T> ParentCollection { set; }
        int Index { set; }
    }

    public abstract class HiraCollection<T> : ScriptableObject//, IEnumerable<T>
#if UNITY_EDITOR
        , IDirtiable
#endif
        where T : ScriptableObject
    {
#if UNITY_EDITOR
        public bool IsDirty { get; set; }
#endif
        [SerializeField] protected T[] collection = { };
        public T[] FirstCollection => collection;
        
        public void Setup(T[] inCollection) => collection = inCollection;

        public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}