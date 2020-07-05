using UnityEngine;

namespace Hiralal.SOVariables.Core
{
    public abstract class ScriptableObjectVariableReference { }

    [System.Serializable]
    public abstract class ScriptableObjectVariableReference<T> : ScriptableObjectVariableReference
    {
        protected ScriptableObjectVariableReference() { }
        protected ScriptableObjectVariableReference(T value) => (useConstant, constant) = (true, value);

        [SerializeField] private bool useConstant = false;
        [SerializeField] private T constant = default;
        public abstract ScriptableObjectVariable<T> Variable { get; }

        public T Value
        {
            get => useConstant ? constant : Variable.Value;
            set
            {
                if (useConstant) constant = value; 
                else Variable.Value = value;
            }
        }
        public static implicit operator T(ScriptableObjectVariableReference<T> original) => original.Value;
    }
}