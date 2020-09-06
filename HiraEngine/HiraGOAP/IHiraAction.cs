namespace Hiralal.GOAP
{
    public interface IHiraAction
    {
        IHiraWorldStateTransition TransitionComponent { get; }
        float Cost { get; }
    }
}