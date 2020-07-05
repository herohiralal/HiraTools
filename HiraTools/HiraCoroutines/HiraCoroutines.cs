using System;
using UnityEngine;

namespace Hiralal.CoroutineTracker
{
    [AddComponentMenu("HiraTools/HiraCoroutines")]
    public class HiraCoroutines : MonoBehaviour
    {
        private static HiraCoroutines _instance = null;

        internal static HiraCoroutines Instance
        {
            get
            {
                if (_instance != null) return _instance;

                if (!Application.isPlaying)
                    throw new NullReferenceException(
                        "Please only use HiraCoroutines while the application is playing.");

                var go = new GameObject(@"[HiraCoroutines]");
                _instance = go.AddComponent<HiraCoroutines>();
                DontDestroyOnLoad(_instance);
                return _instance;
            }
        }
    }
}