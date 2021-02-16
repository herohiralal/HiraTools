/*
 * Name: HiraTweenCore.cs
 * Created By: Rohan Jadav
 * Description: The core generics that can tween anything.
 */

using System;
using System.Collections;
using HiraEngine.CoroutineTracker;
using HiraEngine.Tweeners.Interpolations;

namespace UnityEngine
{
    public static class HiraTweenCore
    {
        private static ulong _index = ulong.MinValue;
        
        public static HiraTweenTracker Start(in this HiraTween tween)
        {
            var tracker = tween.StartLater();
            tracker.Start();
            return tracker;
        }

        public static HiraTweenTracker StartLater(in this HiraTween tween)
        {
            _index++;
            
            var control = HiraTweenControl.Get(in _index);
            control.Initialize(tween.Time, tween.OnIteration, tween.OnCompletion);

            var interpolationMethod = GetInterpolationMethod(tween.TweenType, tween.EaseType);

            var coroutine = UseInterpolator(control, interpolationMethod);

            return new HiraTweenTracker(coroutine, control, _index);
        }

        private static IEnumerator UseInterpolator(HiraTweenControl control, Func<float, float> interpolationMethod)
        {
            while (control.Paused) yield return null;

            var timeSinceStart = Time.deltaTime;
            var onIteration = control.OnIteration;

            while (timeSinceStart < control.Duration)
            {
                if (!control.Paused)
                {
                    onIteration(interpolationMethod(timeSinceStart / control.Duration));
                    timeSinceStart += Time.deltaTime;
                }

                yield return null;
            }

            var completionCallback = control.OnCompletion;
            control.MarkFree();
            onIteration(1);
            completionCallback?.Invoke();
        }

        private static Func<float, float> GetInterpolationMethod(HiraTweenterpolationType tweenType,
            HiraTweenEaseType easeType)
        {
            switch (tweenType)
            {
                case HiraTweenterpolationType.Linear:
                    return LinearTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Quad:
                    return QuadTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Cubic:
                    return CubicTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Quart:
                    return QuartTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Quint:
                    return QuintTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Sine:
                    return SineTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Exponential:
                    return ExponentialTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Circular:
                    return CircularTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Back:
                    return BackTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Elastic:
                    return ElasticTweenterpolator.GetInterpolationMethod(easeType);
                case HiraTweenterpolationType.Bounce:
                    return BounceTweenterpolator.GetInterpolationMethod(easeType);
                default:
                    throw new ArgumentOutOfRangeException(nameof(tweenType), tweenType, null);
            }
        }
    }
}