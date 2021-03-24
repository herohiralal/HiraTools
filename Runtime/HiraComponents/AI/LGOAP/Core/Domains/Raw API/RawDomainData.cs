using System;
using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public readonly unsafe struct RawDomainData
    {
        [NativeDisableUnsafePtrRestriction] private readonly byte* _address;

        public RawDomainData(byte* address) => _address = address;

        public byte LayerCount
        {
            get => *(_address + new RawInsistenceCalculatorsArray(_address).Size);
            private set => *(_address + new RawInsistenceCalculatorsArray(_address).Size) = value;
        }

        public RawInsistenceCalculatorsArray InsistenceCalculators => new RawInsistenceCalculatorsArray(_address);

        public RawLayer this[byte index]
        {
            get
            {
                var countAddress = _address + new RawInsistenceCalculatorsArray(_address).Size;
                var count = *countAddress;
                if (index >= count) throw new IndexOutOfRangeException();

                var current = countAddress + sizeof(byte);

                for (byte i = 0; i < count; i++) current += new RawLayer(current).Size;

                return new RawLayer(current);
            }
        }

        public static RawDomainData Create(
            IEnumerable<IEnumerable<IBlackboardScoreCalculator>> insistenceCalculators,
            byte* address,
            params (IEnumerable<IEnumerable<IBlackboardDecorator>>,
                IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>)[] layers)
        {
            var createdInsistenceCalculators = RawInsistenceCalculatorsArray.Create(insistenceCalculators, address);
            var size = createdInsistenceCalculators.Size;
            size += sizeof(byte); // skip layer count

            byte count = 0;
            foreach (var (targets, actions) in layers)
            {
                size += RawLayer.Create(targets, actions, address + size).Size;
                count++;
            }

            return new RawDomainData(address) {LayerCount = count};
        }

        public static ushort GetSize(
            IEnumerable<IEnumerable<IBlackboardScoreCalculator>> insistenceCalculators,
            params (IEnumerable<IEnumerable<IBlackboardDecorator>>,
                IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>)[] layers)
        {
            ushort size = 0;

            size += RawInsistenceCalculatorsArray.GetSize(insistenceCalculators);

            size += sizeof(byte); // layer count

            foreach (var (targets, actions) in layers)
            {
                size += RawLayer.GetSize(targets, actions);
            }

            return size;
        }
    }
}