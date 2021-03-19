using System;

namespace UnityEngine
{
    public interface IBlackboardComponent
    {
        event Action OnKeyEssentialToDecisionMakingUpdate;
        T GetValue<T>(string keyName) where T : unmanaged;
        T GetValue<T>(ushort keyIndex) where T : unmanaged;
        void SetValue<T>(string keyName, T value) where T : unmanaged;
        void SetValue<T>(ushort keyIndex, T value) where T : unmanaged;
    }
}