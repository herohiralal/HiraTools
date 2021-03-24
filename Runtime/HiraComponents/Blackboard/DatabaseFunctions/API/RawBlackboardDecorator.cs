﻿using HiraEngine.Components.Blackboard.Internal;

namespace HiraEngine.Components.Blackboard
{
	public readonly unsafe struct RawBlackboardDecorator
	{
		private readonly RawBlackboardDatabaseFunction<DecoratorDelegate> _internal;

		public RawBlackboardDecorator(byte* address) =>
			_internal = new RawBlackboardDatabaseFunction<DecoratorDelegate>(address);

		private RawBlackboardDecorator(RawBlackboardDatabaseFunction<DecoratorDelegate> internalFunction) =>
			_internal = internalFunction;

		public byte Size => _internal.Size;

		public bool Execute(byte* blackboard) =>
			_internal.Function.Invoke(blackboard, _internal.State);

		public static RawBlackboardDecorator Create(IBlackboardDecorator from, byte* address) =>
			new RawBlackboardDecorator(RawBlackboardDatabaseFunction<DecoratorDelegate>.Create(from, address));

		public static byte GetSize(IBlackboardDecorator decorator) =>
			RawBlackboardDatabaseFunction<DecoratorDelegate>.GetSizeForDatabaseFunction(decorator);
	}
}