using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hiralal.CoroutineTracker
{
    internal class HiraTimerControl : HiraCoroutineControlGeneric<HiraTimerControl>
    {
        internal void Initialize(float startingTimer, Action onCompletion)
        {
            Timer = startingTimer;
            OnCompletion = onCompletion;
        }
        
        private float Timer { get; set; }

        //====================================================================== COROUTINE INTERFACE

        internal bool TimerNotYetExpired => Timer >= 0;
        
        internal WaitForSecondsRealtime GetWaiter() => new WaitForSecondsRealtime(Timer);

        internal void PunchTimer() => Timer -= Time.deltaTime;

        //====================================================================== TRACKER INTERFACE

        internal float GetTimer(in ulong index) => DoesIndexMatch(in index) ? Timer : 0;

        internal void SetTimer(in ulong index, in float value)
        {
            if (DoesIndexMatch(in index)) Timer = value;
        }
    }
}