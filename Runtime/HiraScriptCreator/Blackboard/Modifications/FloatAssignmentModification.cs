namespace UnityEngine
{
    public class FloatAssignmentModification : AssignmentModification
    {
        [HiraCollectionDropdown(typeof(FloatKey))]
        [SerializeField] private BlackboardKey key = null;

        protected override BlackboardKey Key => key;

        [SerializeField] private float value = 0f;

        protected override string Value => value.ToCode();
        protected override string NonCodeValue => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}