using HiraEngine.Components.Blackboard.Internal;

namespace HiraEngine.Components.Blackboard
{
	public readonly unsafe struct RawBlackboardEffector
	{
		private readonly RawBlackboardDatabaseFunction<EffectorDelegate> _internal;

		public RawBlackboardEffector(byte* address) =>
			_internal = new RawBlackboardDatabaseFunction<EffectorDelegate>(address);

		private RawBlackboardEffector(RawBlackboardDatabaseFunction<EffectorDelegate> internalFunction) =>
			_internal = internalFunction;

		public byte Size => _internal.Size;

		public void Execute(byte* blackboard) =>
			_internal.Function.Invoke(blackboard, _internal.State);

		public static RawBlackboardEffector Create(IBlackboardEffector from, byte* address) =>
			new RawBlackboardEffector(RawBlackboardDatabaseFunction<EffectorDelegate>.Create(from, address));

		public static byte GetSize(IBlackboardEffector effector) =>
			RawBlackboardDatabaseFunction<EffectorDelegate>.GetSizeForDatabaseFunction(effector);
	}
}