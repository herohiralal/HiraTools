using System;
using UnityEngine;
using UnityEngine.Events;

namespace HiraEngine.Tweeners
{
    [AddComponentMenu("HiraTools/Tweeners/Vector2 Tweener")]
    public class Vector2Tweener : TweenerBase<Vector2>
    {
        [Space] [Header("Core")]
        [SerializeField] private Vector2Event target = null;
        [SerializeField] private Vector2Reference startValue = null;
        [SerializeField] private Vector2Reference endValue = null;
        
        protected override UnityEvent<Vector2> Target => target;
        protected override ScriptableObjectVariableReference<Vector2> StartValue => startValue;
        protected override ScriptableObjectVariableReference<Vector2> EndValue => endValue;
        protected override Func<Vector2, Vector2, float, Vector2> InterpolationFunction => Vector2.LerpUnclamped;
    }
}