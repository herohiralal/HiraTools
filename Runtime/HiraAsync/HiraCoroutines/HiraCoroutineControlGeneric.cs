﻿using System.Collections.Generic;

namespace HiraEngine.CoroutineTracker
{
    internal class HiraCoroutineControlGeneric<T> : HiraCoroutineControl where T : HiraCoroutineControlGeneric<T>, new()
    {
        protected HiraCoroutineControlGeneric()
        {
        }
        
        private static readonly List<T> pool = new List<T>();

        internal static T Get(in ulong index)
        {
            var count = pool.Count;

            HiraCoroutineControlGeneric<T> hiraCoroutineControl;
            if (count > 0)
            {
                hiraCoroutineControl = pool[count - 1];
                pool.RemoveAt(count - 1);
            }
            else hiraCoroutineControl = new T();

            // set the appropriate initial state for the object
            hiraCoroutineControl.Paused = false;
            hiraCoroutineControl.IsRunning = false;
            hiraCoroutineControl.Index = index;

            return (T) hiraCoroutineControl;
        }

        internal override void MarkFree()
        {
            pool.Add((T) this);
            base.MarkFree();
        }
    }
}