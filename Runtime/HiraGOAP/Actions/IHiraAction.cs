using Hiralal.GOAP.Transitions;

namespace Hiralal.GOAP.Actions
{
    public interface IHiraAction : IHiraWorldStateTransition
    {
        void OnActionStart();
        void OnActionExecute();
        HiraActionStatus Status { get; }
    }

    public enum HiraActionStatus
    {
        None, Running, Failed, Succeeded
    }
}