namespace UnityEngine
{
    public interface IHiraCreatureMover : IHiraCreatureComponent
    {
        Vector3 Position { get; }
        HiraCreatureMovementMode MovementMode { get; set; }
        void MoveTowards(Vector3 direction);
        void MoveTo(Vector3 position);
    }

    public enum HiraCreatureMovementMode
    {
        None, Positional, Directional
    }
}