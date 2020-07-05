using System;
using Hiralal.SOVariables.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Hiralal.Tweeners
{
    [AddComponentMenu("HiraTools/Tweeners/Quaternion Tweener")]
    public class QuaternionTweener : TweenerBase<Quaternion>
    {
        [Space] [Header("Core")]
        [SerializeField] private QuaternionEvent target = null;
        [SerializeField] private QuaternionReference startValue = null;
        [SerializeField] private QuaternionReference endValue = null;
        
        protected override UnityEvent<Quaternion> Target => target;
        protected override ScriptableObjectVariableReference<Quaternion> StartValue => startValue;
        protected override ScriptableObjectVariableReference<Quaternion> EndValue => endValue;
        protected override Func<Quaternion, Quaternion, float, Quaternion> InterpolationFunction => Quaternion.SlerpUnclamped;
    }
}