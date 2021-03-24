using System;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.Blackboard
{
	public readonly unsafe struct RawBlackboardDatabaseFunction<T> where T : Delegate
	{
		private readonly byte* _address;

		public RawBlackboardDatabaseFunction(byte* address) => _address = address;

		public byte Size
		{
			get => *_address;
			private set => *_address = value;
		}

		public FunctionPointer<T> Function
		{
			get => UnsafeUtility.As<IntPtr, FunctionPointer<T>>(ref *(IntPtr*) (_address + 1));
			private set => UnsafeUtility.As<IntPtr, FunctionPointer<T>>(ref *(IntPtr*) (_address + 1)) = value;
		}

		public byte* State => _address + sizeof(byte) + sizeof(FunctionPointer<T>);

		public static RawBlackboardDatabaseFunction<T> Create(IBlackboardDatabaseFunction<T> from, byte* address)
		{
			var dbf = new RawBlackboardDatabaseFunction<T>(address)
			{
				Size = GetSizeForDatabaseFunction(from),
				Function = from.Function
			};
			@from.AppendMemory(dbf.State);
			return dbf;
		}

		public static byte GetSizeForDatabaseFunction(IBlackboardDatabaseFunction<T> function) =>
			(byte) (sizeof(byte) + sizeof(FunctionPointer<T>) + function.MemorySize);
	}
}