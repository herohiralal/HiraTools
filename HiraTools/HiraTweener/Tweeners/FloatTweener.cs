using System;
using Hiralal.SOVariables.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Hiralal.Tweeners
{
    [AddComponentMenu("HiraTools/Tweeners/Float Tweener")]
    public class FloatTweener : TweenerBase<float>
    {
        [Space] [Header("Core")]
        [SerializeField] private FloatEvent target = null;
        [SerializeField] private FloatReference startValue = null;
        [SerializeField] private FloatReference endValue = null;
        
        protected override UnityEvent<float> Target => target;
        protected override ScriptableObjectVariableReference<float> StartValue => startValue;
        protected override ScriptableObjectVariableReference<float> EndValue => endValue;
        protected override Func<float, float, float, float> InterpolationFunction => Mathf.LerpUnclamped;
    }
}