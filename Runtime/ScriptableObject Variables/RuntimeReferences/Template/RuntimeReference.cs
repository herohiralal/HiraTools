using System;

namespace UnityEngine
{
    public abstract class RuntimeReference<T> : ScriptableObject where T : Object
    {
        public T Value { get; private set; } = null;

        public event Action<T> OnClaim = null;
        public event Action<T> OnRelease = null;

        public void Claim(T owner)
        {
            Value = owner;
            OnClaim?.Invoke(Value);
        }

        public void Release()
        {
            OnRelease?.Invoke(Value);
            Value = null;
        }
    }
}