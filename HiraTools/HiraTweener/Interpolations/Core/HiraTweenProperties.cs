using System;
using HiraTweener.Interpolations.Core;

namespace UnityEngine
{
    public readonly struct HiraTweenProperties
    {
        public HiraTweenProperties(float time,
            Action<float> onIteration,
            HiraTweenterpolationType tweenType = HiraTweenterpolationType.Linear,
            HiraTweenEaseType easeType = HiraTweenEaseType.EaseOut,
            Action onCompletion = null) =>
            (Time, OnIteration, OnCompletion, TweenType, EaseType) =
            (time, onIteration, onCompletion, tweenType, easeType);

        public readonly float Time;
        public readonly Action<float> OnIteration;
        public readonly Action OnCompletion;
        public readonly HiraTweenterpolationType TweenType;
        public readonly HiraTweenEaseType EaseType;
    }
}