using UnityEngine;
using UnityEngine.Events;
using Hiralal.SOVariables.Core;

namespace Hiralal.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Vector2 Listener")]
    public class Vector2ListenerComponent : ScriptableObjectVariableListenerComponent<Vector2>
    {
        [SerializeField] private Vector2Variable variable = null;
        [SerializeField] private Vector2Event onChange = null;
        protected override ScriptableObjectVariable<Vector2> Variable => variable;
        protected override UnityEvent<Vector2> OnChange => onChange;
    }
}