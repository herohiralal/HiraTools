using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;
using HiraEngine.Components.Blackboard.Raw;

namespace HiraEngine.Components.AI.LGOAP.Raw.Internal
{
    public readonly unsafe struct RawAction
    {
        private readonly byte* _address;

        public RawAction(byte* address) => _address = address;

        public ushort Size
        {
            get => *(ushort*) _address;
            private set => *(ushort*) _address = value;
        }

        public void Break(
            out RawBlackboardDecoratorsArray precondition,
            out RawBlackboardScoreCalculatorsArray costCalculator,
            out RawBlackboardEffectorsArray effect)
        {
            var current = _address + sizeof(ushort);
            
            precondition = new RawBlackboardDecoratorsArray(current);
            current += precondition.Size;

            costCalculator = new RawBlackboardScoreCalculatorsArray(current);
            current += costCalculator.Size;

            effect = new RawBlackboardEffectorsArray(current);
        }

        public static RawAction Create(
            IEnumerable<IBlackboardDecorator> precondition,
            IEnumerable<IBlackboardScoreCalculator> costCalculator,
            IEnumerable<IBlackboardEffector> effect,
            byte* address)
        {
            ushort size = sizeof(ushort);
            {
                var createdPrecondition = RawBlackboardDecoratorsArray.Create(precondition, address + size);
                size += createdPrecondition.Size;
            }

            {
                var createdCostCalculator = RawBlackboardScoreCalculatorsArray.Create(costCalculator, address + size);
                size += createdCostCalculator.Size;
            }

            {
                var createdEffect = RawBlackboardEffectorsArray.Create(effect, address + size);
                size += createdEffect.Size;
            }

            return new RawAction(address) {Size = size};
        }

        public static ushort GetSize(
            IEnumerable<IBlackboardDecorator> precondition,
            IEnumerable<IBlackboardScoreCalculator> costCalculator,
            IEnumerable<IBlackboardEffector> effect)
        {
            ushort headerSize = sizeof(ushort);
            headerSize += RawBlackboardDecoratorsArray.GetSize(precondition);
            headerSize += RawBlackboardScoreCalculatorsArray.GetSize(costCalculator);
            headerSize += RawBlackboardEffectorsArray.GetSize(effect);
            return headerSize;
        }
    }
}