using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Raw
{
    public struct RawBlackboardArrayWrapper : IDisposable
    {
        private NativeArray<byte> _container;
        private readonly byte _count;
        private readonly ushort _size;
        
        public RawBlackboardArrayWrapper(byte count, HiraBlackboardTemplate blackboardTemplate)
        {
            _count = count;
            _size = blackboardTemplate.BlackboardSize;
            _container = new NativeArray<byte>(_count * _size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        }

        public void Dispose() => _container.Dispose();

        public unsafe RawBlackboardArray Unwrap() => new RawBlackboardArray((byte*) _container.GetUnsafePtr(), _count, _size);
    }
}