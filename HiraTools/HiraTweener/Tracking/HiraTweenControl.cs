using System;
using System.Collections.Generic;

namespace Hiralal.CoroutineTracker
{
    public class HiraTweenControl
    {
        private static readonly List<HiraTweenControl> pool = new List<HiraTweenControl>();

        private HiraTweenControl() { }
        
        //====================================================================== POOLING

        internal static HiraTweenControl Get(float duration, Action<float> onIteration, Action onCompletion, ulong index)
        {
            var count = pool.Count;
            
            HiraTweenControl hiraTweenControl;
            if (count > 0)
            {
                hiraTweenControl = pool[count - 1];
                pool.RemoveAt(count - 1);
            }
            else hiraTweenControl = new HiraTweenControl();
            
            // set the appropriate initial state for the object
            hiraTweenControl.IsPaused = false;
            hiraTweenControl.Duration = duration;
            hiraTweenControl.OnIteration = onIteration;
            hiraTweenControl.OnCompletion = onCompletion;
            hiraTweenControl.IsRunning = false;
            hiraTweenControl.Index = index;
            
            return hiraTweenControl;
        }

        internal void MarkFree()
        {
            IsRunning = false;
            pool.Add(this);
            Index = ulong.MinValue;
        }
        
        internal ulong Index { get; private set; }

        //====================================================================== CORE FUNCTIONALITY
        internal bool IsRunning { get; private set; }

        internal Action<float> OnIteration = null;
        internal Action OnCompletion = null;
        internal void MarkStarted() => IsRunning = true;
        
        //====================================================================== PAUSING / RESUMING
        
        internal float Duration { get; private set; }

        internal bool IsPaused { get; set; } = false;
    }
}