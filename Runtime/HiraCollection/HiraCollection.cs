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
    }

    public abstract class HiraCollection<T1, T2> : HiraCollection<T1>
        where T1 : ScriptableObject
        where T2 : ScriptableObject
    {
        [SerializeField] protected T2[] secondCollection = { };
        public T2[] SecondCollection => secondCollection;

        public void Setup(T1[] inCollection1, T2[] inCollection2) =>
            (collection, secondCollection) = (inCollection1, inCollection2);
    }
}