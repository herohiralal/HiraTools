namespace UnityEngine
{
    public class IntegerAssignmentModification : AssignmentModification
    {
        [HiraCollectionDropdown(typeof(INumericalKey))]
        [SerializeField] private BlackboardKey key = null;

        protected override BlackboardKey Key => key;

        [SerializeField] private int value = 0;

        protected override string Value => value.ToCode();
    }
}