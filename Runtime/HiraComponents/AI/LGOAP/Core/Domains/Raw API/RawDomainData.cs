using System;
using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;
using HiraEngine.Components.Blackboard.Raw;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public readonly unsafe struct RawDomainData
    {
        [NativeDisableUnsafePtrRestriction] private readonly byte* _address;

        public RawDomainData(byte* address) => _address = address;

        public RawInsistenceCalculatorsArray InsistenceCalculators => new RawInsistenceCalculatorsArray(_address);

        public RawBlackboardEffectorsArray Restarters => new RawBlackboardEffectorsArray(_address + InsistenceCalculators.Size);

        public byte LayerCount
        {
	        get => *(_address + InsistenceCalculators.Size + Restarters.Size);
	        private set => *(_address + InsistenceCalculators.Size + Restarters.Size) = value;
        }

        public RawLayer this[byte index]
        {
            get
            {
                var countAddress = _address + InsistenceCalculators.Size + Restarters.Size;
                if (index >= *countAddress) throw new IndexOutOfRangeException();

                var current = countAddress + sizeof(byte);

                for (byte i = 0; i < index; i++) current += new RawLayer(current).Size;

                return new RawLayer(current);
            }
        }

        public static RawDomainData Create(
            IEnumerable<IEnumerable<IBlackboardScoreCalculator>> insistenceCalculators,
            IEnumerable<IBlackboardEffector> restarters,
            byte* address,
            params (IEnumerable<IEnumerable<IBlackboardDecorator>>,
                IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>)[] layers)
        {
            var createdInsistenceCalculators = RawInsistenceCalculatorsArray.Create(insistenceCalculators, address);
            var size = createdInsistenceCalculators.Size;

            var createdRestarters = RawBlackboardEffectorsArray.Create(restarters, address + size);
            size += createdRestarters.Size;
            
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
            IEnumerable<IBlackboardEffector> restarters,
            params (IEnumerable<IEnumerable<IBlackboardDecorator>>,
                IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>)[] layers)
        {
            ushort size = 0;

            size += RawInsistenceCalculatorsArray.GetSize(insistenceCalculators);

            size += RawBlackboardEffectorsArray.GetSize(restarters);

            size += sizeof(byte); // layer count

            foreach (var (targets, actions) in layers)
            {
                size += RawLayer.GetSize(targets, actions);
            }

            return size;
        }
    }
}