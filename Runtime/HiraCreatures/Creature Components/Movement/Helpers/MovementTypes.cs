using HiraEngine.Components.Movement;
using HiraEngine.Components.Movement.Internal;

namespace UnityEngine
{
    public static class MovementTypes
    {
        public static IDirectionalMovementAssistant GetDirectionalMovementAssistant() =>
            new DirectionalMovementAssistant();
    }
}