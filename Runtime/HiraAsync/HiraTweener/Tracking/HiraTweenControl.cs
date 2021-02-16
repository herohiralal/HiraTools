using System;
using UnityEngine;

namespace HiraEngine.CoroutineTracker
{
    internal class HiraTweenControl : HiraCoroutineControlGeneric<HiraTweenControl>
    {
        internal void Initialize(float duration, Action<float> onIteration, Action onCompletion)
        {
            Duration = duration;
            OnIteration = onIteration;
            OnCompletion = onCompletion;
        }

        internal Action<float> OnIteration { get; private set; } = null;

        internal float Duration { get; private set; }

        internal HiraTweenTracker Chain(in ulong index, in HiraTween tween)
        {
            if (!DoesIndexMatch(in index)) throw new NullReferenceException("Invalid index.");

            var tracker = tween.StartLater();
            OnCompletion += tracker.Start;
            return tracker;
        }
    }
}