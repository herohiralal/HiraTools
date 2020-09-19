using UnityEngine;
using UnityEngine.Events;
using HiraEngine.SOVariables.Core;

namespace HiraEngine.SOVariables.ListenerComponents
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Byte Listener")]
    public class ByteListenerComponent : ScriptableObjectVariableListenerComponent<byte>
    {
        [SerializeField] private ByteVariable variable = null;
        [SerializeField] private ByteEvent onChange = null;
        protected override ScriptableObjectVariable<byte> Variable => variable;
        protected override UnityEvent<byte> OnChange => onChange;
    }
}