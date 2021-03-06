﻿using System;
using System.Collections.Generic;
using HiraEngine.Components.AI.LGOAP.Raw.Internal;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public readonly unsafe struct RawActionsArray
    {
        [NativeDisableUnsafePtrRestriction] private readonly byte* _address;

        public RawActionsArray(byte* address) => _address = address;

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

        public RawAction this[byte index]
        {
	        get
	        {
		        if (index >= Count) throw new IndexOutOfRangeException();

		        var current = First;
		        for (byte i = 0; i < index; i++) current += new RawAction(current).Size;
		        return new RawAction(current);
	        }
        }

        public static RawActionsArray Create(
            IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions,
            byte* address)
        {
            byte count = 0;
            ushort size = sizeof(ushort) + sizeof(byte);
            foreach (var (precondition, costCalculator, effect) in actions)
            {
                var currentElement = RawAction.Create(precondition, costCalculator, effect, address + size);

                size += currentElement.Size;
                count++;
            }

            return new RawActionsArray(address) {Size = size, Count = count};
        }

        public static ushort GetSize(
            IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])> actions)
        {
            ushort headerSize = sizeof(ushort) + sizeof(byte);
            foreach (var (precondition, costCalculator, effect) in actions)
                headerSize += RawAction.GetSize(precondition, costCalculator, effect);
            return headerSize;
        }
    }
}