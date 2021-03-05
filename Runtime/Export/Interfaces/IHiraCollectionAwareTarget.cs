namespace UnityEngine
{
    public interface IHiraCollectionAwareTarget
    {
        HiraCollection ParentCollection { get; set; }
        int Index { get; set; }
    }
}