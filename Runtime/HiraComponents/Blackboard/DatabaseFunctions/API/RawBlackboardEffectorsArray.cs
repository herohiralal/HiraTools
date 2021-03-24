using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;

namespace HiraEngine.Components.Blackboard
{
    public readonly unsafe struct RawBlackboardEffectorsArray
    {
        private readonly RawBlackboardDatabaseFunctionsArray<EffectorDelegate> _internal;

        public RawBlackboardEffectorsArray(byte* address) =>
            _internal = new RawBlackboardDatabaseFunctionsArray<EffectorDelegate>(address);

        public RawBlackboardEffectorsArray(RawBlackboardDatabaseFunctionsArray<EffectorDelegate> internalFunctionsArray) =>
            _internal = internalFunctionsArray;

        public ushort Size => _internal.Size;

        public void Execute(byte* blackboard)
        {
            var count = _internal.Count;
            var current = _internal.First;
            for (var i = 0; i < count; i++)
            {
                var currentElement = new RawBlackboardEffector(current);
                current += currentElement.Size;

                currentElement.Execute(blackboard);
            }
        }

        public static RawBlackboardEffectorsArray Create(IEnumerable<IBlackboardEffector> from, byte* address) =>
            new RawBlackboardEffectorsArray(RawBlackboardDatabaseFunctionsArray<EffectorDelegate>.Create(from, address));

        public static ushort GetSize(IEnumerable<IBlackboardEffector> decorators) =>
            RawBlackboardDatabaseFunctionsArray<EffectorDelegate>.GetSize(decorators);
    }
}