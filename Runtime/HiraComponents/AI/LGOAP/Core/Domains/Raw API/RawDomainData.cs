using System;
using System.Collections.Generic;
using System.Linq;
using HiraEngine.Components.Blackboard.Internal;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace HiraEngine.Components.AI.LGOAP.Raw
{
    public unsafe struct RawDomainData : IDisposable
    {
        private NativeArray<byte> _data;
        [NativeDisableUnsafePtrRestriction] private readonly byte* _address;

        private RawDomainData(NativeArray<byte> data)
        {
            _data = data;
            _address = (byte*) data.GetUnsafePtr();
        }

        public void Dispose() => _data.Dispose();

        public RawInsistenceCalculatorsArray InsistenceCalculators => new RawInsistenceCalculatorsArray(_address);

        public byte LayerCount
        {
	        get => *(_address + InsistenceCalculators.Size);
	        private set => *(_address + InsistenceCalculators.Size) = value;
        }

        public RawLayer this[byte index]
        {
            get
            {
                var countAddress = _address + InsistenceCalculators.Size;
                if (index >= *countAddress) throw new IndexOutOfRangeException();

                var current = countAddress + sizeof(byte);

                for (byte i = 0; i < index; i++) current += new RawLayer(current).Size;

                return new RawLayer(current);
            }
        }

        private static void Compile(
            byte* address,
            IEnumerable<IEnumerable<IBlackboardScoreCalculator>> insistenceCalculators,
            IEnumerable<(IEnumerable<IEnumerable<IBlackboardDecorator>>,
                IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>)> layers)
        {
            var createdInsistenceCalculators = RawInsistenceCalculatorsArray.Create(insistenceCalculators, address);
            var size = createdInsistenceCalculators.Size;
            
            size += sizeof(byte); // skip layer count

            foreach (var (targets, actions) in layers)
            {
                size += RawLayer.Create(targets, actions, address + size).Size;
            }
        }

        private static ushort GetSize(
            IEnumerable<IEnumerable<IBlackboardScoreCalculator>> insistenceCalculators,
            IEnumerable<(IEnumerable<IEnumerable<IBlackboardDecorator>>,
                IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>)> layers)
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

        public static RawDomainData Create(Goal[] goals, Action[] actions, params IntermediateGoal[][] layers)
        {
            // arrange data for compiler
            var layerData = new (IEnumerable<IEnumerable<IBlackboardDecorator>>, IEnumerable<(IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>)[layers.Length + 1];

            var insistenceCalculators = goals.Select(g => g.InsistenceCalculators).ToArray();
            layerData[0].Item1 = goals.Select(g => g.Targets).ToArray();

            for (var i = 0; i < layers.Length; i++)
            {
                layerData[i].Item2 = layers[i].Select<IntermediateGoal, (IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>(ig =>
                    (ig.Precondition, ig.CostCalculator, ig.Effect)).ToArray();

                layerData[i + 1].Item1 = layers[i].Select(ig => ig.Targets).ToArray();
            }

            layerData[layers.Length].Item2 = actions.Select<Action, (IBlackboardDecorator[], IBlackboardScoreCalculator[], IBlackboardEffector[])>(a =>
                (a.Precondition, a.CostCalculator, a.Effect)).ToArray();

            // determine required size
            var size = GetSize(insistenceCalculators, layerData);
            
            // allocate
            var rawData = new NativeArray<byte>(size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            
            // compile
            Compile((byte*) rawData.GetUnsafePtr(), insistenceCalculators, layerData);

            // finalize
            return new RawDomainData(rawData) {LayerCount = (byte) (layers.Length + 1)};
        }
    }
}