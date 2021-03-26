namespace HiraEngine.Components.AI.Internal
{
    public class AutoFailExecutable : Executable
    {
        public static readonly AutoFailExecutable INSTANCE = new AutoFailExecutable();
        
        public override void OnExecutionStart()
        {
        }

        public override ExecutionStatus Execute(float deltaTime) => ExecutionStatus.Failed;

        public override void OnExecutionSuccess()
        {
        }

        public override void OnExecutionFailure()
        {
        }

        public override void OnExecutionAbort()
        {
        }
    }
}