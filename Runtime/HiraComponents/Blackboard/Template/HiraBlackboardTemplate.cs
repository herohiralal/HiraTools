using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Assertions;

namespace UnityEngine
{
    [Flags]
    public enum BlackboardKeyTraits : byte
    {
        None = 0,
        InstanceSynced = 1 << 0,
        EssentialToDecisionMaking = 1 << 1
    }

    [CreateAssetMenu]
    public class HiraBlackboardTemplate : HiraCollection<HiraBlackboardKey>, IInitializable
    {
        [NonSerialized] private ushort _cachedTotalSize = 0;
        [NonSerialized] private Dictionary<string, ushort> _keyIndices = null;
        [NonSerialized] private Dictionary<ushort, BlackboardKeyTraits> _keyTraits = null;
        [NonSerialized] private NativeArray<byte> _template = default;

        public ushort this[string keyName] => _keyIndices[keyName];
        public BlackboardKeyTraits this[ushort index] => _keyTraits[index];

        private ushort TotalSize
        {
            get
            {
                ushort size = 0;
                foreach (var key in Collection1)
                {
                    size += key.SizeInBytes;
                }

                return size;
            }
        }

        public unsafe void Initialize<T>(ref T _)
        {
            _keyIndices = new Dictionary<string, ushort>();
            _keyTraits = new Dictionary<ushort, BlackboardKeyTraits>();

            _cachedTotalSize = TotalSize;
            var sortedKeys = Collection1.OrderBy(k => k.SizeInBytes);
            _template = new NativeArray<byte>(_cachedTotalSize, Allocator.Persistent);
            var templatePtr = (byte*) _template.GetUnsafePtr();

            ushort index = 0;
            foreach (var key in sortedKeys)
            {
                var keyName = key.name;

                // cache the string-to-id hash table
                if (!_keyIndices.ContainsKey(keyName))
                    _keyIndices.Add(keyName, index);
                else
                {
                    Debug.LogError($"Blackboard contains multiple keys named {keyName}.", this);
                    _keyIndices[keyName] = index;
                }

                // update the index on the key itself
                key.Index = index;

                // update instance syncing data
                _keyTraits.Add(index, key.Traits);

                // set the default value
                key.SetDefault(templatePtr + index);

                // get the next index
                index += key.SizeInBytes;
            }

            Assert.AreEqual(_cachedTotalSize, index);
        }

        public void Shutdown()
        {
            _template.Dispose();

            _keyTraits = null;
            _keyIndices = null;
        }

        public NativeArray<byte> GetNewBlackboard()
        {
            var output = new NativeArray<byte>(_cachedTotalSize, Allocator.Persistent);
            _template.CopyTo(output);
            return output;
        }

        public unsafe delegate void InstanceSyncKeyUpdateDelegate(ushort keyIndex, bool isEssentialToDecisionMaking, byte* value, byte size);

        public event InstanceSyncKeyUpdateDelegate OnInstanceSyncKeyUpdate = delegate { };

        public unsafe void UpdateInstanceSyncedKey<T>(ushort keyIndex, T value) where T : unmanaged
        {
            *(T*) ((byte*) _template.GetUnsafePtr() + keyIndex) = value;

            OnInstanceSyncKeyUpdate.Invoke(
                keyIndex, 
                this[keyIndex].HasFlag(BlackboardKeyTraits.EssentialToDecisionMaking), 
                (byte*) &value, 
                (byte) sizeof(T));
        }
    }
}