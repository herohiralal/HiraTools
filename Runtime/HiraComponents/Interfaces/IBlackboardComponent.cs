using System;
using Unity.Collections;

namespace UnityEngine
{
    public interface IBlackboardComponent
    {
        HiraBlackboardTemplate Template { get; }
        NativeArray<byte> Data { get; }
        bool BroadcastKeyUpdateEvents { get; set; }
        event Action OnKeyEssentialToDecisionMakingUpdate;
        T GetValue<T>(string keyName) where T : unmanaged;
        T GetValue<T>(ushort keyIndex) where T : unmanaged;
        void SetValue<T>(string keyName, T value) where T : unmanaged;
        void SetValue<T>(ushort keyIndex, T value) where T : unmanaged;
    }

    public readonly struct BlackboardComponentBroadcastDisabler : IDisposable
    {
        private readonly IBlackboardComponent _target;
        private readonly bool _isMaster;
        
        public BlackboardComponentBroadcastDisabler(IBlackboardComponent target)
        {
            _target = target;
            _isMaster = target.BroadcastKeyUpdateEvents;
            _target.BroadcastKeyUpdateEvents = false;
        }

        public void Dispose()
        {
            if (_isMaster) _target.BroadcastKeyUpdateEvents = true;
        }
    }
}