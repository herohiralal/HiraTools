namespace HiraEngine.Components.Blackboard.Internal
{
	public unsafe interface IBlackboardDatabaseFunction<T> where T : System.Delegate
	{
		byte MemorySize { get; }
		void AppendMemory(byte* stream);
		Unity.Burst.FunctionPointer<T> Function { get; }
	}
}