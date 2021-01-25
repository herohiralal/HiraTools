namespace UnityEngine
{
    public interface IIndividualQuery
    {
        string Condition { get; }
    }

    public abstract class IndividualQuery : ScriptableObject, IIndividualQuery
    {
        public abstract string Condition { get; }
    }
}