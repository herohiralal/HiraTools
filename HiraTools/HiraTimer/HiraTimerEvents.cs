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
        private static ulong _index = ulong.MinValue;
        
        public static IHiraTimerTracker RequestPing(Action onTimerFinish, float time, bool startAutomatically = true, bool ignoreTimeScale = false)
        {
            _index++;
            
            var control = HiraTimerControl.Get(_index);
            control.Initialize(time, onTimerFinish);
            
            var enumerator = ignoreTimeScale
                ? UnscaledTimerPing(control)
                : ScaledTimerPing(control);
            
            var tracker = new HiraTimerTracker(enumerator, control, _index);

            if (startAutomatically) tracker.Start();
            
            return tracker;
        }

        private static IEnumerator ScaledTimerPing(HiraTimerControl control)
        {
            while (control.TimerNotYetExpired)
            {
                if (!control.Paused) control.PunchTimer();

                yield return null;
            }

            control.OnCompletion();
            control.MarkFree();
        }

        private static IEnumerator UnscaledTimerPing(HiraTimerControl control)
        {
            while (control.Paused) yield return null;
            yield return control.GetWaiter();
            control.OnCompletion();
            control.MarkFree();
        }
    }
}