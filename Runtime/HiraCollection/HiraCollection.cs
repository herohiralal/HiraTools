namespace UnityEngine
{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
    public interface IHiraCollectionEditorInterface
    {
        public Object[] Collection1 { get; }
    }
#endif
    
    public interface ICollectionAwareTarget<T>
    {
        HiraCollection<T> ParentCollection { set; }
        int Index { set; }
    }

    public abstract class HiraCollection<T> : ScriptableObject
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        , IDirtiable
        , IHiraCollectionEditorInterface
#endif
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        public bool IsDirty { get; set; }
        Object[] IHiraCollectionEditorInterface.Collection1 => collection1 as Object[];
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

    public abstract class HiraCollection<T1, T2, T3> : HiraCollection<T1, T2>
    {
        [SerializeField] protected T3[] collection3 = { };
        public T3[] Collection3 => collection3;

        public void Setup(T1[] inCollection1, T2[] inCollection2, T3[] inCollection3) =>
            (collection1, collection2, collection3) = (inCollection1, inCollection2, inCollection3);
    }
}