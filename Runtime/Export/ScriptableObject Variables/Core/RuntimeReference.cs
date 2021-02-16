using System;

namespace UnityEngine
{
    public abstract class RuntimeReference<T> : ScriptableObject where T : Object
    {
        public T Value { get; private set; } = null;

        public event Action<T> OnClaim = null;
        public event Action<T> OnRelease = null;

        internal void Claim(T owner)
        {
            Value = owner;
            OnClaim?.Invoke(Value);
        }

        internal void Release()
        {
            OnRelease?.Invoke(Value);
            Value = null;
        }
    }
}