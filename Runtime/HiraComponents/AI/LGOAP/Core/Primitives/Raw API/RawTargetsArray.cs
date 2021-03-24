using System;
using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public readonly unsafe struct RawTargetsArray
    {
        [NativeDisableUnsafePtrRestriction] private readonly byte* _address;

        public RawTargetsArray(byte* address) => _address = address;

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

        public RawBlackboardDecoratorsArray this[byte index]
        {
            get
            {
                var count = Count;
                if (index >= count) throw new IndexOutOfRangeException();

                var current = _address + sizeof(ushort) + sizeof(byte);
                
                for (byte i = 0; i < count; i++) current += new RawBlackboardDecoratorsArray(current).Size;

                return new RawBlackboardDecoratorsArray(current);
            }
        }

        public static RawTargetsArray Create(IEnumerable<IEnumerable<IBlackboardDecorator>> targets, byte* address)
        {
            byte count = 0;
            ushort size = sizeof(ushort) + sizeof(byte);
            foreach (var target in targets)
            {
                var currentElement = RawBlackboardDecoratorsArray.Create(target, address + size);

                size += currentElement.Size;
                count++;
            }

            return new RawTargetsArray(address) {Size = size, Count = count};
        }

        public static ushort GetSize(IEnumerable<IEnumerable<IBlackboardDecorator>> targets)
        {
            ushort headerSize = sizeof(ushort) + sizeof(byte);
            foreach (var target in targets) headerSize += RawBlackboardDecoratorsArray.GetSize(target);
            return headerSize;
        }
    }
}