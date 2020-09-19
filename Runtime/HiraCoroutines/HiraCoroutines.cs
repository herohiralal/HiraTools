using System;
using UnityEngine;

namespace HiraEngine.CoroutineTracker
{
    [AddComponentMenu("HiraTools/HiraCoroutines")]
    public class HiraCoroutines : MonoBehaviour
    {
        private static HiraCoroutines _instance = null;
        private static bool _initialized = false;

        internal static HiraCoroutines Instance
        {
            get
            {
                if (_initialized) return _instance;

                if (!Application.isPlaying)
                    throw new NullReferenceException(
                        "Please only use HiraCoroutines while the application is playing.");

                var go = new GameObject(@"[HiraCoroutines]");
                (_initialized, _instance) = (true, go.AddComponent<HiraCoroutines>());
                DontDestroyOnLoad(_instance);
                return _instance;
            }
        }

        private void OnDestroy()
        {
            if (_instance == this) (_initialized, _instance) = (false, null);
        }
    }
}