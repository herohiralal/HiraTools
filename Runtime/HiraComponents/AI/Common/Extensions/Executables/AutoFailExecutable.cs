namespace HiraEngine.Components.AI.Internal
{
    public class AutoFailExecutable : Executable
    {
        private AutoFailExecutable()
        {
        }

        public static readonly AutoFailExecutable INSTANCE = new AutoFailExecutable();
        public override ExecutionStatus Execute(float deltaTime) => ExecutionStatus.Failed;
    }
}