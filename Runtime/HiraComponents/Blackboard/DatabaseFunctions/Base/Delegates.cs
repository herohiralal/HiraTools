namespace HiraEngine.Components.Blackboard.Internal
{
	public unsafe delegate bool DecoratorDelegate(byte* blackboard, byte* memory);

	public unsafe delegate void EffectorDelegate(byte* blackboard, byte* memory);
}