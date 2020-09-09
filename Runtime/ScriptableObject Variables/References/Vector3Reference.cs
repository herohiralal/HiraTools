using System;
using Hiralal.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class Vector3Reference : ScriptableObjectVariableReference<Vector3>
    {
        public Vector3Reference() { }
        public Vector3Reference(Vector3 value) : base(value) { }

        [SerializeField] private Vector3Variable variable = null;
        public override ScriptableObjectVariable<Vector3> Variable => variable;
    }
}