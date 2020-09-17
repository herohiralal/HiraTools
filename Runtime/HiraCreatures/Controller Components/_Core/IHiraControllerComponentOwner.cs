namespace UnityEngine
{
    public interface IHiraControllerComponentOwner<out T> where T : IHiraControllerComponent
    {
        T Component { get; }
    }
}