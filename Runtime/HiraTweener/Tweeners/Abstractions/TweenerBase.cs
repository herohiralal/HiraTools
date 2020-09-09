using System;
using Hiralal.SOVariables.Core;
using HiraTweener.Interpolations.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Hiralal.Tweeners
{
    public abstract class TweenerBase<T> : Tweener
    {
        protected abstract UnityEvent<T> Target { get; }
        protected abstract ScriptableObjectVariableReference<T> StartValue { get; }
        protected abstract ScriptableObjectVariableReference<T> EndValue { get; }
        protected abstract Func<T, T, float, T> InterpolationFunction { get; }

        //====================================================================== CORE INTERFACE

        public sealed override void StartTweenForward()
        {
            if (isActiveAndEnabled)
                Tween(StartValue.Value,
                    EndValue.Value,
                    easing,
                    OnForwardCompletion);
        }

        public sealed override void StartTweenBackward()
        {
            if (isActiveAndEnabled)
                Tween(EndValue.Value,
                    StartValue.Value,
                    invertEasingWhenTweeningBackwards
                        ? InverseEasing
                        : easing,
                    OnBackwardCompletion);
        }

        public sealed override void Pause()
        {
            if (TrackerIsValid) CurrentTracker.Pause();
        }

        public sealed override void Resume()
        {
            if (TrackerIsValid && CurrentTracker.IsPaused) CurrentTracker.Resume();
        }

        public sealed override void Stop()
        {
            if (TrackerIsValid) CurrentTracker.Stop();
        }

        public sealed override void SetToStart() => Target.Invoke(StartValue.Value);

        public sealed override void SetToEnd() => Target.Invoke(EndValue.Value);

        //====================================================================== HELPERS

        private void Tween(T startValue, T endValue, HiraTweenEaseType easeType, Action callback)
        {
            Stop();

            CurrentTracker = new HiraTween(duration,
                f => Target.Invoke(InterpolationFunction(startValue, endValue, f)),
                type, easeType, callback).Start();
        }

        private void OnForwardCompletion()
        {
            onForwardCompletion.Invoke();
            switch (repeat)
            {
                case HiraTweenRepeatType.Off:
                    break;
                case HiraTweenRepeatType.Loop:
                    StartTweenForward();
                    break;
                case HiraTweenRepeatType.PingPong:
                    StartTweenBackward();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnBackwardCompletion()
        {
            onBackwardCompletion.Invoke();
            switch (repeat)
            {
                case HiraTweenRepeatType.Off:
                    break;
                case HiraTweenRepeatType.Loop:
                    StartTweenBackward();
                    break;
                case HiraTweenRepeatType.PingPong:
                    StartTweenForward();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}