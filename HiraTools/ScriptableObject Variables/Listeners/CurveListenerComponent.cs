using UnityEngine;
using UnityEngine.Events;
using Hiralal.SOVariables.Core;

namespace Hiralal.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Curve Listener")]
    public class CurveListenerComponent : ScriptableObjectVariableListenerComponent<AnimationCurve>
    {
        [SerializeField] private CurveVariable variable = null;
        [SerializeField] private CurveEvent onChange = null;
        protected override ScriptableObjectVariable<AnimationCurve> Variable => variable;
        protected override UnityEvent<AnimationCurve> OnChange => onChange;
    }
}