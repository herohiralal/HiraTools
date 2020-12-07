using HiraEngine.SOVariables.Core;
using UnityEngine;
using UnityEngine.Events;

namespace HiraEngine.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Vector2Int Listener")]
    public class Vector2IntListenerComponent : ScriptableObjectVariableListenerComponent<Vector2Int>
    {
        [SerializeField] private Vector2IntVariable variable = null;
        [SerializeField] private Vector2IntEvent onChange = null;
        protected override ScriptableObjectVariable<Vector2Int> Variable => variable;
        protected override UnityEvent<Vector2Int> OnChange => onChange;
    }
}