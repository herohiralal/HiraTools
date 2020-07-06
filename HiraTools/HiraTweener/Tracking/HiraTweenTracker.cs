using System.Collections;
using Hiralal.CoroutineTracker;

namespace UnityEngine
{
    public interface IHiraTweenTracker
    {
        bool IsValid { get; }
        bool HasStarted { get; }
        bool IsPaused { get; }
        void Start();
        void Resume();
        void Pause();
        void Stop(bool withOnEnableCallback = false);
        IHiraTweenTracker Chain(in HiraTween tween);
    }
    
    internal readonly struct HiraTweenTracker : IHiraTweenTracker
    {
        internal HiraTweenTracker(IEnumerator coroutine, HiraTweenControl control, ulong index) =>
            (this.coroutine, this.control, this.index) = (coroutine, control, index);

        private readonly ulong index;
        private readonly IEnumerator coroutine;
        private readonly HiraTweenControl control;
        
        //====================================================================== QUERIES

        public bool IsValid => index == control.Index; 
        public bool HasStarted => control.IsRunning;
        public bool IsPaused => control.IsPaused;
        
        //====================================================================== COMMANDS
        
        public void Start()
        {
            if (!IsValid)
            {
                LogInvalidTrackerUsage();
                return;
            }

            if (HasStarted)
            {
                LogAlreadyRunning();
                return;
            }
            
            control.MarkStarted();
            HiraCoroutines.Instance.StartCoroutine(coroutine);
        }

        public void Resume()
        {
            if (!IsValid)
            {
                LogInvalidTrackerUsage();
                return;
            }
            
            control.IsPaused = false;
        }

        public void Pause()
        {
            if (!IsValid)
            {
                LogInvalidTrackerUsage();
                return;
            }
            
            control.IsPaused = true;
        }

        public void Stop(bool withOnEnableCallback = false)
        {
            if (!IsValid)
            {
                LogInvalidTrackerUsage();
                return;
            }

            if (!HasStarted)
            {
                LogNotRunning();
                return;
            }
            
            HiraCoroutines.Instance.StopCoroutine(coroutine);
            if (withOnEnableCallback) control.OnCompletion?.Invoke();
            control.MarkFree();
        }

        public IHiraTweenTracker Chain(in HiraTween tween)
        {
            if (!IsValid)
            {
                LogInvalidTrackerUsage();
                return null;
            }

            var tracker = tween.StartLater();
            control.OnCompletion += tracker.Start;
            return tracker;
        }
        
        //====================================================================== EXCEPTION HANDLING

        private static void LogInvalidTrackerUsage() => Debug.LogError("Invalid tracker being used.");

        private static void LogAlreadyRunning() => Debug.LogWarning("The requested tween is already running.");

        private static void LogNotRunning() => Debug.LogWarning("The requested tween has not been started yet.");
    }
}