using System;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class GradientReference : ScriptableObjectVariableReference<Gradient>
    {
        public GradientReference() { }
        public GradientReference(Gradient value) : base(value) { }

        [SerializeField] private GradientVariable variable = null;
        public override ScriptableObjectVariable<Gradient> Variable => variable;
    }
}