namespace HiraEngine.Components.AI
{
    public enum ExecutionStatus
    {
        InProgress,
        Succeeded,
        Failed
    }

    public abstract class Executable
    {
        public virtual void OnExecutionStart()
        {
        }

        public virtual ExecutionStatus Execute(float deltaTime) => ExecutionStatus.Succeeded;

        public virtual void OnExecutionSuccess()
        {
        }

        public virtual void OnExecutionFailure()
        {
        }

        public virtual void OnExecutionAbort()
        {
        }
    }
}