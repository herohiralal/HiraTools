using Unity.Collections;

namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public enum PlannerResultType : byte
	{
		Uninitialized = 0,
		Failure = 1,
		Unchanged = 2,
		Success = 3
	}

	public struct PlannerResult
	{
		public PlannerResult(byte bufferSize, Allocator allocator) =>
			Container = new NativeArray<byte>(bufferSize + 3, allocator, NativeArrayOptions.UninitializedMemory)
			{
				[0] = 0,
				[1] = (byte) PlannerResultType.Uninitialized,
                [2] = 0
			};

		public void Dispose() => Container.Dispose();

		public NativeArray<byte> Container;

		public byte Count
		{
			get => Container[0];
			set => Container[0] = value;
		}

		public PlannerResultType ResultType
		{
			get => (PlannerResultType) Container[1];
			set => Container[1] = (byte) value;
		}

		public byte BufferSize => (byte) (Container.Length - 3);

        public byte CurrentIndex
        {
            get => Container[2];
            set => Container[2] = value;
        }

		public byte this[byte index]
		{
			get => Container[index + 3];
			set => Container[index + 3] = value;
		}

        public bool CanPop => CurrentIndex < Count;

        public byte Pop() => this[CurrentIndex++];
	}
}