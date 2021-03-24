using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;
using HiraEngine.Components.Blackboard.Raw.Internal;

namespace HiraEngine.Components.Blackboard.Raw
{
    public readonly unsafe struct RawBlackboardDecoratorsArray
    {
        private readonly RawBlackboardDatabaseFunctionsArray<DecoratorDelegate> _internal;

        public RawBlackboardDecoratorsArray(byte* address) =>
            _internal = new RawBlackboardDatabaseFunctionsArray<DecoratorDelegate>(address);

        private RawBlackboardDecoratorsArray(RawBlackboardDatabaseFunctionsArray<DecoratorDelegate> internalFunctionsArray) =>
            _internal = internalFunctionsArray;

        public ushort Size => _internal.Size;

        public bool Execute(byte* blackboard)
        {
            var count = _internal.Count;
            var current = _internal.First;
            var result = true;
            for (byte i = 0; i < count; i++)
            {
                var currentElement = new RawBlackboardDecorator(current);
                current += currentElement.Size;

                result = result && currentElement.Execute(blackboard);
            }

            return result;
        }

        public byte CalculateHeuristic(byte* blackboard)
        {
            var count = _internal.Count;
            var current = _internal.First;
            byte result = 0;
            for (var i = 0; i < count; i++)
            {
                var currentElement = new RawBlackboardDecorator(current);
                current += currentElement.Size;

                result += (byte) (currentElement.Execute(blackboard) ? 0 : 1);
            }

            return result;
        }

        public static RawBlackboardDecoratorsArray Create(IEnumerable<IBlackboardDecorator> from, byte* address) =>
            new RawBlackboardDecoratorsArray(RawBlackboardDatabaseFunctionsArray<DecoratorDelegate>.Create(from, address));

        public static ushort GetSize(IEnumerable<IBlackboardDecorator> decorators) =>
            RawBlackboardDatabaseFunctionsArray<DecoratorDelegate>.GetSize(decorators);
    }
}