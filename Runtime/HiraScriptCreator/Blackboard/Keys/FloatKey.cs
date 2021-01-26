namespace UnityEngine
{
    public class FloatKey : NumericalKey
    {
        [SerializeField] private float defaultValue = 0.0f;
        protected override string KeyType => "float";
        protected override string DefaultValue => $"{defaultValue}f";
    }
}