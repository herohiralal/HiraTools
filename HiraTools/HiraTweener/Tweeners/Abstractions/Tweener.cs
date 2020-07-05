using System;
using HiraTweener.Interpolations.Core;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine
{
    public abstract class Tweener : MonoBehaviour
    {
        [Space][Header("Callbacks")]
        [SerializeField] protected UnityEvent onForwardCompletion = null;
        [SerializeField] protected UnityEvent onBackwardCompletion = null;
        
        [Space][Header("Tween Style")]
        [SerializeField] protected float duration = default;
        [SerializeField] protected HiraTweenterpolationType type = default;
        [SerializeField] protected HiraTweenEaseType easing = default;
        [SerializeField] protected HiraTweenRepeatType repeat = HiraTweenRepeatType.Off; 

        protected HiraTweenEaseType InverseEasing =>
            easing switch
            {
                HiraTweenEaseType.EaseIn => HiraTweenEaseType.EaseOut,
                HiraTweenEaseType.EaseOut => HiraTweenEaseType.EaseIn,
                HiraTweenEaseType.EaseInOut => easing,
                _ => throw new ArgumentOutOfRangeException()
            };

        protected IHiraTweenTracker CurrentTracker = null;
        protected bool TrackerIsValid => CurrentTracker != null && CurrentTracker.IsValid;
        public bool IsPaused => TrackerIsValid && CurrentTracker.IsPaused;

        public abstract void StartTweenForward();
        public abstract void StartTweenBackward();
        public abstract void Pause();
        public abstract void Resume();
        public abstract void Stop();
        public abstract void SetToStart();
        public abstract void SetToEnd();
    }

    public enum HiraTweenRepeatType
    {
        Off, Loop, PingPong
    }
}