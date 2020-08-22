using System;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class CurveReference : ScriptableObjectVariableReference<AnimationCurve>
    {
        public CurveReference() { }
        public CurveReference(AnimationCurve value) : base(value) { }

        [SerializeField] private CurveVariable variable = null;
        public override ScriptableObjectVariable<AnimationCurve> Variable => variable;
    }
}