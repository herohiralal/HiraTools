namespace UnityEngine
{
    public class BoolComparisonQuery : ScriptableObject, IIndividualQuery
    {
        [SerializeField] private float weight = 1;
        public float Weight => weight;
        
        [HiraCollectionDropdown(typeof(BooleanKey))]
        [SerializeField] protected BlackboardKey key = null;

        [SerializeField] private bool invert = false;

        private void OnValidate()
        {
            if (key != null) name = $"{key.name} is {!invert}";
        }

        public string Condition => $"({(invert?"!":"")}blackboard->{key.name})";
    }
}