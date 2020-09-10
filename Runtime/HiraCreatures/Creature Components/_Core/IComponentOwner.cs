namespace UnityEngine
{
    public interface IComponentOwner<out T> where T : IHiraCreatureComponent
    {
        T Component { get; }
    }
}