﻿/*
 * Name: HiraTimerEvents.cs
 * Created By: Rohan Jadav
 * Description: 
 */

using System;
using System.Collections;
using Hiralal.CoroutineTracker;

namespace UnityEngine
{
    public static class HiraTimerEvents
    {
        public static IHiraTimerTracker RequestPing(Action onTimerFinish, float time, bool startAutomatically = true, bool ignoreTimeScale = false)
        {
            var control = HiraTimerControl.Get(time);
            control.IsPaused = !startAutomatically;
            var enumerator = ignoreTimeScale
                ? UnscaledTimerPing(onTimerFinish, control)
                : ScaledTimerPing(onTimerFinish, control);
            var coroutine = HiraCoroutines.Instance.StartCoroutine(enumerator);
            return new HiraTimerTracker(coroutine, control);
        }

        private static IEnumerator ScaledTimerPing(Action onTimerFinish, HiraTimerControl control)
        {
            while (control.Timer >= 0)
            {
                if (!control.IsPaused) control.Timer -= Time.deltaTime;

                yield return null;
            }

            onTimerFinish();
            control.MarkFree();
        }

        private static IEnumerator UnscaledTimerPing(Action onTimerFinish, HiraTimerControl control)
        {
            while (control.IsPaused) yield return null;
            yield return new WaitForSecondsRealtime(control.Timer);
            onTimerFinish();
            control.MarkFree();
        }
    }
}