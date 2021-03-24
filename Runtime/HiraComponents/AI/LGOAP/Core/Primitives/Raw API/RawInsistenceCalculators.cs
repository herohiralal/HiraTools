using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public readonly unsafe struct RawInsistenceCalculatorsArray
    {
        [NativeDisableUnsafePtrRestriction] private readonly byte* _address;

        public RawInsistenceCalculatorsArray(byte* address) => _address = address;

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

        public static RawInsistenceCalculatorsArray Create(IEnumerable<IEnumerable<IBlackboardScoreCalculator>> insistenceCalculators, byte* address)
        {
            byte count = 0;
            ushort size = sizeof(ushort) + sizeof(byte);
            foreach (var insistenceCalculator in insistenceCalculators)
            {
                var currentElement = RawBlackboardScoreCalculatorsArray.Create(insistenceCalculator, address + size);

                size += currentElement.Size;
                count++;
            }

            return new RawInsistenceCalculatorsArray(address) {Size = size, Count = count};
        }

        public static ushort GetSize(IEnumerable<IEnumerable<IBlackboardScoreCalculator>> insistenceCalculators)
        {
            ushort headerSize = sizeof(ushort) + sizeof(byte);
            foreach (var insistenceCalculator in insistenceCalculators) headerSize += RawBlackboardScoreCalculatorsArray.GetSize(insistenceCalculator);
            return headerSize;
        }
    }
}