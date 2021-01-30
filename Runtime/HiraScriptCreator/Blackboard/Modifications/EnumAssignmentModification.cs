namespace UnityEngine
{
    public abstract class EnumAssignmentModification<T> : AssignmentModification where T : System.Enum
    {
        [HiraCollectionDropdown(typeof(EnumKey))]
        [SerializeField] private BlackboardKey key = null;

        protected override BlackboardKey Key => key;

        [SerializeField] private T value = default;

        protected override string Value => value.ToCode();

        protected override string NonCodeValue => value.ToString().Replace(", ", " and ");
    }
}