﻿using System.Collections.Generic;

namespace Hiralal.CoroutineTracker
{
    internal class HiraTimerControl
    {
        private static readonly List<HiraTimerControl> pool = new List<HiraTimerControl>();

        private HiraTimerControl() { }        
        internal static HiraTimerControl Get(float startingTimer)
        {
            var count = pool.Count;
            
            HiraTimerControl hiraTimerControl;
            if (count > 0)
            {
                hiraTimerControl = pool[count - 1];
                pool.RemoveAt(count - 1);
            }
            else hiraTimerControl = new HiraTimerControl();

            hiraTimerControl.Timer = startingTimer;
            return hiraTimerControl;
        }
        
        internal float Timer { get; set; }

        internal bool IsPaused { get; set; } = false;

        internal void MarkFree() => pool.Add(this);
    }
}