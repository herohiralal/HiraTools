using Hiralal.SOVariables.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Hiralal.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Quaternion Listener")]
    public class QuaternionListenerComponent : ScriptableObjectVariableListenerComponent<Quaternion>
    {
        [SerializeField] private QuaternionVariable variable = null;
        [SerializeField] private QuaternionEvent onChange = null;
        protected override ScriptableObjectVariable<Quaternion> Variable => variable;
        protected override UnityEvent<Quaternion> OnChange => onChange;
    }
}