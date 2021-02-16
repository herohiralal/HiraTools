namespace UnityEngine
{
    public interface IHiraCollectionAwareTarget
    {
        HiraCollection ParentCollection { set; }
        int Index { set; }
    }
}