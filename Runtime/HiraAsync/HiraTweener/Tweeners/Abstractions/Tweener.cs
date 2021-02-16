using System;
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
        [SerializeField] protected HiraTweenPlayType playOnAwake = HiraTweenPlayType.Off;
        [SerializeField] protected bool invertEasingWhenTweeningBackwards = true;

        protected void Awake()
        {
            switch (playOnAwake)
            {
                case HiraTweenPlayType.Off:
                    break;
                case HiraTweenPlayType.Forward:
                    StartTweenForward();
                    break;
                case HiraTweenPlayType.Backward:
                    StartTweenBackward();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected HiraTweenEaseType InverseEasing
        {
            get
            {
                switch (easing)
                {
                    case HiraTweenEaseType.EaseIn:
                        return HiraTweenEaseType.EaseOut;
                    case HiraTweenEaseType.EaseOut:
                        return HiraTweenEaseType.EaseIn;
                    case HiraTweenEaseType.EaseInOut:
                        return easing;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected HiraTweenTracker CurrentTracker = default;
        protected bool TrackerIsValid => CurrentTracker.IsValid;
        public bool IsPaused => TrackerIsValid && CurrentTracker.IsPaused;

        public abstract void StartTweenForward();
        public abstract void StartTweenBackward();
        public abstract void Pause();
        public abstract void Resume();
        public abstract void Stop();
        public abstract void SetToStart();
        public abstract void SetToEnd();
    }
}