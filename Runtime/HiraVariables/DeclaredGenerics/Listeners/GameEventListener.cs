﻿using UnityEngine.Events;

namespace UnityEngine.Internal
{
    [AddComponentMenu("HiraTools/ScriptableObject Variables/Listeners/Game Event Listener")]
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent gameEvent = null;
        [SerializeField] private UnityEvent onGameEventRaise = null;

        private void OnEnable() => gameEvent.OnRaise += onGameEventRaise.Invoke;
        private void OnDisable() => gameEvent.OnRaise -= onGameEventRaise.Invoke;
    }
}