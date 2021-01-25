namespace UnityEngine
{
    public class IntegerEqualityQuery : IndividualQuery
    {
        [HiraCollectionDropdown(typeof(IntegerKey))] [SerializeField]
        private IntegerKey key = null;

        [SerializeField] private int targetValue = 0;

        public override string Condition =>
            $"(blackboard.{key.name} == {targetValue})";
    }
}