namespace UnityEngine
{
    public abstract class AssignmentModification : ScriptableObject, IIndividualModification
    {
        protected abstract BlackboardKey Key { get; }
        protected abstract string NonCodeValue { get; }
        protected abstract string Value { get; }
        public string Modification => $"{Key.name} = {Value}";

        private void OnValidate()
        {
            if (Key != null) name = $"Set {Key.name} to {NonCodeValue}";
        }
    }
}