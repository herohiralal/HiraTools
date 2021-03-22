using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public enum PlannerResultType : byte
	{
		Uninitialized = 0,
		Failure = 1,
		Unchanged = 2,
		Success = 3
	}

	public unsafe struct PlannerResult
	{
		public PlannerResult(byte bufferSize, Allocator allocator) =>
			_container = new NativeArray<byte>(bufferSize + 2, allocator, NativeArrayOptions.UninitializedMemory)
			{
				[0] = 0,
				[1] = (byte) PlannerResultType.Uninitialized
			};

		public void Dispose() => _container.Dispose();

		private NativeArray<byte> _container;

		public byte Count
		{
			get => _container[0];
			set => _container[0] = value;
		}

		public PlannerResultType ResultType
		{
			get => (PlannerResultType) _container[1];
			set => _container[1] = (byte) value;
		}

		public byte BufferSize => (byte) (_container.Length - 2);

		public byte this[byte index]
		{
			get => _container[index + 2];
			set => _container[index + 2] = value;
		}

		public byte* GetUnsafePtr() => 2 + (byte*) _container.GetUnsafePtr();

		public byte* GetUnsafeReadOnlyPtr() => 2 + (byte*) _container.GetUnsafeReadOnlyPtr();

		public void CopyTo(PlannerResult other) => _container.CopyTo(other._container);
	}
}