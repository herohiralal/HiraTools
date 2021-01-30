namespace UnityEngine
{
    public class BooleanAssignmentModification : AssignmentModification
    {
        [HiraCollectionDropdown(typeof(BooleanKey))]
        [SerializeField] private BlackboardKey key = null;

        protected override BlackboardKey Key => key;

        [SerializeField] private bool value = false;

        protected override string Value => value.ToCode();
        protected override string NonCodeValue => value.ToString();
    }
}