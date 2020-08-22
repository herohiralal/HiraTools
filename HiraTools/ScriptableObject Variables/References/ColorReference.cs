using System;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class ColorReference : ScriptableObjectVariableReference<Color>
    {
        public ColorReference() { }
        public ColorReference(Color value) : base(value) { }

        [SerializeField] private ColorVariable variable = null;
        public override ScriptableObjectVariable<Color> Variable => variable;
    }
}