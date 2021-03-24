using System;
using System.Collections.Generic;

namespace HiraEngine.Components.Blackboard.Internal
{
    public readonly unsafe struct RawBlackboardDatabaseFunctionsArray<T> where T : Delegate
    {
        private readonly byte* _address;

        public RawBlackboardDatabaseFunctionsArray(byte* address) => _address = address;

        public ushort Size
        {
            get => *(ushort*) _address;
            private set => *(ushort*) _address = value;
        }

        public byte Count
        {
            get => *(_address + sizeof(ushort));
            private set => *(_address + sizeof(ushort)) = value;
        }

        public byte* First => _address + sizeof(ushort) + sizeof(byte);

        public static RawBlackboardDatabaseFunctionsArray<T> Create(IEnumerable<IBlackboardDatabaseFunction<T>> functions, byte* address)
        {
            byte count = 0;
            ushort size = sizeof(ushort) + sizeof(byte);
            foreach (var function in functions)
            {
                var currentElement = RawBlackboardDatabaseFunction<T>.Create(function, address + size);

                size += currentElement.Size;
                count++;
            }

            return new RawBlackboardDatabaseFunctionsArray<T>(address) {Size = size, Count = count};
        }

        public static ushort GetSize(IEnumerable<IBlackboardDatabaseFunction<T>> functions)
        {
            ushort headerSize = sizeof(ushort) + sizeof(byte);
            foreach (var function in functions) headerSize += RawBlackboardDatabaseFunction<T>.GetSize(function);
            return headerSize;
        }
    }
}