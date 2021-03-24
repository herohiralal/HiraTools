using System;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.Blackboard.Raw
{
    public readonly unsafe struct RawBlackboardArray
    {
        [NativeDisableUnsafePtrRestriction] private readonly byte* _address;
        private readonly byte _count;
        private readonly ushort _size;

        public RawBlackboardArray(byte* address, byte count, ushort size)
        {
            _address = address;
            _count = count;
            _size = size;
        }

        public ushort BlackboardSize => _size;
        public ushort Count => _count;
        public byte* this[byte index] => index >= _count ? throw new IndexOutOfRangeException() : _address + (_size * index);
    }
}