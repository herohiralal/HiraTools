using System;
using HiraTweener.Interpolations.Core;

namespace UnityEngine
{
    public readonly struct HiraTween
    {
        // Generic Constructor
        public HiraTween(float time,
            Action<float> onIteration,
            HiraTweenterpolationType tweenType = HiraTweenterpolationType.Linear,
            HiraTweenEaseType easeType = HiraTweenEaseType.EaseIn,
            Action onCompletion = null)
        {
            Time = time;
            OnIteration = onIteration;
            TweenType = tweenType;
            EaseType = easeType;
            OnCompletion = onCompletion;
        }
        
        // Color Tween Constructor
        public HiraTween(Action<Color> setter,
            Color a,
            Color b,
            float time,
            HiraTweenterpolationType tweenType = HiraTweenterpolationType.Linear,
            HiraTweenEaseType easeType = HiraTweenEaseType.EaseIn,
            Action onCompletion = null)
        {
            Time = time;
            OnIteration = t => setter(Color.LerpUnclamped(a, b, t));
            OnCompletion = onCompletion;
            TweenType = tweenType;
            EaseType = easeType;
        }
        
        // Euler Tween Constructor
        public HiraTween(Action<Vector3> setter,
            Quaternion a,
            Quaternion b,
            float time,
            HiraTweenterpolationType tweenType = HiraTweenterpolationType.Linear,
            HiraTweenEaseType easeType = HiraTweenEaseType.EaseIn,
            Action onCompletion = null)
        {
            Time = time;
            var (x, y) = (a.eulerAngles, b.eulerAngles);
            OnIteration = t => setter(Vector3.SlerpUnclamped(x, y, t));
            OnCompletion = onCompletion;
            TweenType = tweenType;
            EaseType = easeType;
        }
        
        // Float Tween Constructor
        public HiraTween(Action<float> setter,
            float a,
            float b,
            float time,
            HiraTweenterpolationType tweenType = HiraTweenterpolationType.Linear,
            HiraTweenEaseType easeType = HiraTweenEaseType.EaseIn,
            Action onCompletion = null)
        {
            Time = time;
            OnIteration = t => setter(Mathf.LerpUnclamped(a, b, t));
            OnCompletion = onCompletion;
            TweenType = tweenType;
            EaseType = easeType;
        }
        
        // Quaternion Tween Constructor
        public HiraTween(Action<Quaternion> setter,
            Quaternion a,
            Quaternion b,
            float time,
            HiraTweenterpolationType tweenType = HiraTweenterpolationType.Linear,
            HiraTweenEaseType easeType = HiraTweenEaseType.EaseIn,
            Action onCompletion = null)
        {
            Time = time;
            OnIteration = t => setter(Quaternion.SlerpUnclamped(a, b, t));
            OnCompletion = onCompletion;
            TweenType = tweenType;
            EaseType = easeType;
        }
        
        // Vector2 Tween Constructor
        public HiraTween(Action<Vector2> setter,
            Vector2 a,
            Vector2 b,
            float time,
            HiraTweenterpolationType tweenType = HiraTweenterpolationType.Linear,
            HiraTweenEaseType easeType = HiraTweenEaseType.EaseIn,
            Action onCompletion = null)
        {
            Time = time;
            OnIteration = t => setter(Vector2.LerpUnclamped(a, b, t));
            OnCompletion = onCompletion;
            TweenType = tweenType;
            EaseType = easeType;
        }
        
        // Vector3 Tween Constructor
        public HiraTween(Action<Vector3> setter,
            Vector3 a,
            Vector3 b,
            float time,
            HiraTweenterpolationType tweenType = HiraTweenterpolationType.Linear,
            HiraTweenEaseType easeType = HiraTweenEaseType.EaseIn,
            Action onCompletion = null)
        {
            Time = time;
            OnIteration = t => setter(Vector3.LerpUnclamped(a, b, t));
            OnCompletion = onCompletion;
            TweenType = tweenType;
            EaseType = easeType;
        }

        public readonly float Time;
        public readonly Action<float> OnIteration;
        public readonly Action OnCompletion;
        public readonly HiraTweenterpolationType TweenType;
        public readonly HiraTweenEaseType EaseType;
    }
}