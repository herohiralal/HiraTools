using System.Collections;
using Hiralal.CoroutineTracker;

namespace UnityEngine
{
    public readonly struct HiraTimerTracker
    {
        internal HiraTimerTracker(IEnumerator coroutine, HiraTimerControl control, ulong index) =>
            (this._coroutine, this._control, this._index) = (coroutine, control, index);

        private readonly ulong _index;
        private readonly IEnumerator _coroutine;
        private readonly HiraTimerControl _control;

        //====================================================================== QUERIES

        public bool IsValid => _control.IsValid(in _index);
        public bool HasStarted => _control.HasStarted(in _index);
        public bool IsPaused => _control.IsPaused(in _index);

        //====================================================================== COMMANDS

        public void Start() => _control.Start(in _index, _coroutine);

        public void Resume() => _control.Resume(in _index);

        public void Pause() => _control.Pause(in _index);

        public void Stop(bool withOnCompletionCallback = false) => _control.Stop(in _index, _coroutine, withOnCompletionCallback);

        //====================================================================== STATE

        public float Timer
        {
            get => _control.GetTimer(in _index);
            set => _control.SetTimer(in _index, in value);
        }
    }
}