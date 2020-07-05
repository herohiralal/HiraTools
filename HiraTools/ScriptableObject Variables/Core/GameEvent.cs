using System;

namespace UnityEngine
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Hiralal/ScriptableObject Variables/Game Event")]
    public class GameEvent : ScriptableObject
    {
        public event Action OnRaise = default;

        public void Raise() => OnRaise?.Invoke();
    }
}