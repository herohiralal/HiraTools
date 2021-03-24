using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;
using HiraEngine.Components.Blackboard.Raw.Internal;

namespace HiraEngine.Components.Blackboard.Raw
{
    public readonly unsafe struct RawBlackboardScoreCalculatorsArray
    {
        private readonly RawBlackboardDatabaseFunctionsArray<DecoratorDelegate> _internal;

        public RawBlackboardScoreCalculatorsArray(byte* address) =>
            _internal = new RawBlackboardDatabaseFunctionsArray<DecoratorDelegate>(address);

        public RawBlackboardScoreCalculatorsArray(RawBlackboardDatabaseFunctionsArray<DecoratorDelegate> internalFunctionsArray) =>
            _internal = internalFunctionsArray;

        public ushort Size => _internal.Size;

        public float Execute(byte* blackboard)
        {
            var count = _internal.Count;
            var current = _internal.First;
            var result = 0f;
            for (byte i = 0; i < count; i++)
            {
                var currentElement = new RawBlackboardScoreCalculator(current);
                current += currentElement.Size;

                result += currentElement.Execute(blackboard);
            }

            return result;
        }

        public static RawBlackboardScoreCalculatorsArray Create(IEnumerable<IBlackboardScoreCalculator> from, byte* address) =>
            new RawBlackboardScoreCalculatorsArray(RawBlackboardDatabaseFunctionsArray<DecoratorDelegate>.Create(from, address));

        public static ushort GetSize(IEnumerable<IBlackboardScoreCalculator> scoreCalculators) =>
            RawBlackboardDatabaseFunctionsArray<DecoratorDelegate>.GetSize(scoreCalculators);
    }
}