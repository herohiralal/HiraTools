namespace HiraEngine.Components.AI
{
    public class EmptyExecutable : IExecutable
    {
        public static readonly EmptyExecutable INSTANCE = new EmptyExecutable();

        public void OnExecutionStart()
        {
        }

        public ExecutionStatus Execute(float deltaTime) => ExecutionStatus.InProgress;

        public void OnExecutionSuccess()
        {
        }

        public void OnExecutionFailure()
        {
        }

        public void OnExecutionAbort()
        {
        }
    }
}