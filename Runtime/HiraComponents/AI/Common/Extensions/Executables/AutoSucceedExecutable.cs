namespace HiraEngine.Components.AI.Internal
{
	public class AutoSucceedExecutable : Executable
	{
		private AutoSucceedExecutable()
		{		    
		}

		public static readonly AutoSucceedExecutable INSTANCE = new AutoSucceedExecutable();
		public override ExecutionStatus Execute(float deltaTime) => ExecutionStatus.Succeeded;
	}
}