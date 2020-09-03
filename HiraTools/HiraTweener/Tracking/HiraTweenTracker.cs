using System.Collections;
using Hiralal.CoroutineTracker;

namespace UnityEngine
{
    public readonly struct HiraTweenTracker
    {
        internal HiraTweenTracker(IEnumerator coroutine, HiraTweenControl control, ulong index) =>
            (this.coroutine, this.control, this.index) = (coroutine, control, index);

        private readonly ulong index;
        private readonly IEnumerator coroutine;
        private readonly HiraTweenControl control;

        //====================================================================== QUERIES

        public bool IsValid => control != null && control.IsValid(in index);
        public bool HasStarted => control.HasStarted(in index);
        public bool IsPaused => control.IsPaused(in index);

        //====================================================================== COMMANDS

        public void Start() => control.Start(in index, coroutine);

        public void Resume() => control.Resume(in index);

        public void Pause() => control.Pause(in index);

        public void Stop(bool withOnCompletionCallback = false) =>
            control.Stop(in index, coroutine, withOnCompletionCallback);

        public HiraTweenTracker Chain(in HiraTween tween) => control.Chain(in index, in tween);
    }
}