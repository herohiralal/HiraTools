namespace UnityEngine
{
    public class BoolComparisonQuery : ScriptableObject, IIndividualQuery
    {
        [SerializeField] private float weight = 1;
        public float Weight => weight;
        
        [HiraCollectionDropdown(typeof(BooleanKey))]
        [SerializeField] protected BlackboardKey key = null;

        [SerializeField] private bool invert = false;


        public string Condition => $"({(invert?"!":"")}blackboard->{key.name})";
    }
}