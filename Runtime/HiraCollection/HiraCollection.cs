namespace UnityEngine
{
    public interface ICollectionAwareTarget<T>
    {
        HiraCollection<T> ParentCollection { set; }
        int Index { set; }
    }

    public abstract class HiraCollection<T> : ScriptableObject
#if UNITY_EDITOR
        , IDirtiable
#endif
    {
#if UNITY_EDITOR
        public bool IsDirty { get; set; }
#endif
        [SerializeField] protected T[] collection1 = { };
        public T[] Collection1 => collection1;

        public void Setup(T[] inCollection) => collection1 = inCollection;
    }

    public abstract class HiraCollection<T1, T2> : HiraCollection<T1>
    {
        [SerializeField] protected T2[] collection2 = { };
        public T2[] Collection2 => collection2;

        public void Setup(T1[] inCollection1, T2[] inCollection2) => 
            (collection1, collection2) = (inCollection1, inCollection2);
    }
}