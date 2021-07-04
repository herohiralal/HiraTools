namespace HiraEngine.Components.AI.Internal
{
    public class EmptyExecutable : Executable
    {
        private EmptyExecutable()
        {
        }

        public static readonly EmptyExecutable INSTANCE = new EmptyExecutable();
        public override ExecutionStatus Execute(float deltaTime) => ExecutionStatus.InProgress;
    }
}