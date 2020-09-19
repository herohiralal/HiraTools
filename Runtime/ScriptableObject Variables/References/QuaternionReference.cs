using System;
using HiraEngine.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class QuaternionReference : ScriptableObjectVariableReference<Quaternion>
    {
        public QuaternionReference() { }
        public QuaternionReference(Quaternion value) : base(value) { }

        [SerializeField] private QuaternionVariable variable = null;
        public override ScriptableObjectVariable<Quaternion> Variable => variable;
    }
}