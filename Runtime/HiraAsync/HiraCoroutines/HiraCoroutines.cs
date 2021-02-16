using UnityEngine;

namespace HiraEngine.CoroutineTracker
{
    [HiraManager]
    [AddComponentMenu("")]
    public class HiraCoroutines : MonoBehaviour
    {
        public static HiraCoroutines Instance { get; [JetBrains.Annotations.UsedImplicitly] private set; }
    }
}