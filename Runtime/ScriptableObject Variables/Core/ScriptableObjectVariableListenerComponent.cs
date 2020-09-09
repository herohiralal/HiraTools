using UnityEngine;
using UnityEngine.Events;

namespace Hiralal.SOVariables.Core
{
    public abstract class ScriptableObjectVariableListenerComponent : MonoBehaviour { }

    public abstract class ScriptableObjectVariableListenerComponent<T> : ScriptableObjectVariableListenerComponent
    {
        protected abstract ScriptableObjectVariable<T> Variable { get; }
        protected abstract UnityEvent<T> OnChange { get; }

        protected void OnEnable() => Variable.OnValueChange += OnChange.Invoke;
        protected void OnDisable() => Variable.OnValueChange -= OnChange.Invoke;
    }
}