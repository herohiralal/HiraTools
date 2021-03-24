using HiraEngine.Components.Blackboard.Internal;

namespace HiraEngine.Components.Blackboard
{
	public readonly unsafe struct RawBlackboardScoreCalculator
	{
		private readonly RawBlackboardDatabaseFunction<DecoratorDelegate> _internal;

		public RawBlackboardScoreCalculator(byte* address) =>
			_internal = new RawBlackboardDatabaseFunction<DecoratorDelegate>(address);

		private RawBlackboardScoreCalculator(RawBlackboardDatabaseFunction<DecoratorDelegate> internalFunction) =>
			_internal = internalFunction;

		public byte Size => _internal.Size;

		public float Execute(byte* blackboard) =>
			_internal.Function.Invoke(blackboard, _internal.State + sizeof(float)) ? *(float*) _internal.State : 0;

		public static RawBlackboardScoreCalculator Create(IBlackboardScoreCalculator from, byte* address) =>
			new RawBlackboardScoreCalculator(RawBlackboardDatabaseFunction<DecoratorDelegate>.Create(from, address));

		public static byte GetSize(IBlackboardScoreCalculator calculator) =>
			RawBlackboardDatabaseFunction<DecoratorDelegate>.GetSizeForDatabaseFunction(calculator);
	}
}