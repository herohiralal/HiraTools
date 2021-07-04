namespace HiraEngine.Components.AI
{
    public enum ExecutionStatus
    {
        InProgress,
        Succeeded,
        Failed
    }

    public abstract class Executable : System.IDisposable
    {
        public virtual void OnExecutionStart()
        {
        }

        public abstract ExecutionStatus Execute(float deltaTime);

        public virtual void OnExecutionSuccess()
        {
        }

        public virtual void OnExecutionFailure()
        {
        }

        public virtual void OnExecutionAbort()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}