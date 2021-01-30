namespace UnityEngine
{
    public abstract class AssignmentModification : ScriptableObject, IIndividualModification
    {
        protected abstract BlackboardKey Key { get; }
        protected abstract string Value { get; }
        public string Modification => $"blackboard->{Key.name} = {Value}";
    }
}