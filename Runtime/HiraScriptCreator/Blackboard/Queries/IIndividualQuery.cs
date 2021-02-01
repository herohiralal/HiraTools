namespace UnityEngine
{
    public interface IIndividualQuery
    {
        string Condition { get; }
        int Weight { get; }
    }
}