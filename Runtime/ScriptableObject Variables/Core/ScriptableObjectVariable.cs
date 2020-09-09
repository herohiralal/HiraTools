using System;
using UnityEngine;
// ReSharper disable Unity.RedundantSerializeFieldAttribute

namespace Hiralal.SOVariables.Core
{
    public abstract class ScriptableObjectVariable : ScriptableObject { }
    public abstract class ScriptableObjectVariable<T> : ScriptableObjectVariable
    {
        [SerializeField] private T _value = default;
        public event Action<T> OnValueChange = default;

        public T Value
        {
            get => _value;
            set
            {
                OnValueChange?.Invoke(value);
                this._value = value;
            }
        }

        public static implicit operator T(ScriptableObjectVariable<T> original) => original.Value;

        public override string ToString() => $"{_value} ({name})";
    }
}