namespace HiraEngine.Components.AI
{
    public enum ExecutionStatus
    {
        InProgress, Succeeded, Failed
    }
    
    public interface IExecutable
    {
        void OnExecutionStart();
        ExecutionStatus Execute();
        void OnExecutionFinish();
        void OnExecutionAbort();
    }
}