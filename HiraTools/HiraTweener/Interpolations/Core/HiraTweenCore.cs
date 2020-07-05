/*
 * Name: HiraTweenCore.cs
 * Created By: Rohan Jadav
 * Description: The core generics that can tween anything.
 */

using System;
using System.Collections;
using Hiralal.CoroutineTracker;
using HiraTweener.Interpolations.Core;
using HiraTweener.Interpolations.Tweenterpolators;

namespace UnityEngine
{
    public static class HiraTweenCore
    {
        private static ulong _index = ulong.MinValue;
        
        public static IHiraTweenTracker Interpolate(in HiraTweenProperties properties)
        {
            var tracker = InterpolateLater(in properties);
            tracker.Start();
            return tracker;
        }

        public static IHiraTweenTracker InterpolateLater(in HiraTweenProperties properties)
        {
            _index++;
            
            var control = HiraTweenControl.Get(properties.Time, properties.OnIteration, properties.OnCompletion, _index);

            var interpolationMethod = GetInterpolationMethod(properties.TweenType, properties.EaseType);

            var coroutine = UseInterpolator(control, interpolationMethod);

            return new HiraTweenTracker(coroutine, control, _index);
        }

        private static IEnumerator UseInterpolator(HiraTweenControl control, Func<float, float> interpolationMethod)
        {
            while (control.IsPaused) yield return null;

            var timeSinceStart = Time.deltaTime;
            var onIteration = control.OnIteration;

            while (timeSinceStart < control.Duration)
            {
                if (!control.IsPaused)
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
            HiraTweenEaseType easeType) =>
            tweenType switch
            {
                HiraTweenterpolationType.Linear => LinearTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Quad => QuadTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Cubic => CubicTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Quart => QuartTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Quint => QuintTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Sine => SineTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Exponential => ExponentialTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Circular => CircularTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Back => BackTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Elastic => ElasticTweenterpolator.GetInterpolationMethod(easeType),
                HiraTweenterpolationType.Bounce => BounceTweenterpolator.GetInterpolationMethod(easeType),
                _ => throw new ArgumentOutOfRangeException(nameof(tweenType), tweenType, null)
            };
    }
}