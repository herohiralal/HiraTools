using System;
using HiraEngine.SOVariables.Core;

namespace UnityEngine
{
    [Serializable]
    public class Vector2IntReference : ScriptableObjectVariableReference<Vector2Int>
    {
        public Vector2IntReference() { }
        public Vector2IntReference(Vector2Int value) : base(value) { }

        [SerializeField] private Vector2IntVariable variable = null;
        public override ScriptableObjectVariable<Vector2Int> Variable => variable;
    }
}