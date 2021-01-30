namespace UnityEngine
{
    public interface IIndividualQuery
    {
        string Condition { get; }
        float Weight { get; }
    }
}