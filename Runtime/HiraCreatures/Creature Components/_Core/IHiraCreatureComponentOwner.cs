namespace UnityEngine
{
    public interface IHiraCreatureComponentOwner<out T> where T : IHiraCreatureComponent
    {
        T Component { get; }
    }
}