using System;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class FloatReference : ScriptableObjectVariableReference<float>
    {
        public FloatReference() { }
        public FloatReference(float value) : base(value) { }

        [SerializeField] private FloatVariable variable = null;
        public override ScriptableObjectVariable<float> Variable => variable;
    }
}