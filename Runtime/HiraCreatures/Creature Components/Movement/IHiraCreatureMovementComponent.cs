namespace UnityEngine
{
    public interface IHiraCreatureMovementComponent : IHiraCreatureComponent
    {
        HiraCreatureMovementMode MovementMode { get; set; }
        void MoveTowards(Vector3 direction);
        void MoveTo(Vector3 position);
    }

    public enum HiraCreatureMovementMode
    {
        None, Positional, Directional
    }
}