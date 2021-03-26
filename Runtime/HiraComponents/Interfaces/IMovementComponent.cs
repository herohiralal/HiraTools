namespace UnityEngine
{
    public interface IMovementComponent
    {
        Vector3 Position { get; }
        HiraCreatureMovementMode MovementMode { get; set; }
        void MoveTowards(Vector3 direction);
        void MoveTo(Vector3 position, float stoppingDistance);
        float RemainingDistance { get; }
        void StopMovingToDestination();
    }

    public enum HiraCreatureMovementMode
    {
        None, Positional, Directional
    }
}