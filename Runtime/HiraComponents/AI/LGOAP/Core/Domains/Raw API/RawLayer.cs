using System.Collections.Generic;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public readonly unsafe struct RawLayer
    {
        [NativeDisableUnsafePtrRestriction] private readonly byte* _address;

        public RawLayer(byte* address) => _address = address;

        public ushort Size
        {
            get
            {
                ushort size = 0;

                var targets = new RawTargetsArray(_address);
                size += targets.Size;

                var actions = new RawActionsArray(_address + size);
                size += actions.Size;

                return size;
            }
        }

        public void Break(out RawTargetsArray targets, out RawActionsArray actions)
        {
            targets = new RawTargetsArray(_address);
            actions = new RawActionsArray(_address + targets.Size);
        }

        public static RawLayer Create(
            IEnumerable<IEnumerable<IBlackboardDecorator>> targets,
            IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions,
            byte* address)
        {
            var createdTargets = RawTargetsArray.Create(targets, address);

            var targetsSize = createdTargets.Size;

            RawActionsArray.Create(actions, address + targetsSize);

            return new RawLayer(address);
        }

        public static ushort GetSize(
            IEnumerable<IEnumerable<IBlackboardDecorator>> targets,
            IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions)
        {
            ushort size = 0;
            size += RawTargetsArray.GetSize(targets);
            size += RawActionsArray.GetSize(actions);
            return size;
        }
    }
}