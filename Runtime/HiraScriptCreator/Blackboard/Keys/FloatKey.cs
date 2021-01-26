namespace UnityEngine
{
    public class FloatKey : BlackboardKey, INumericalKey
    {
        [SerializeField] private float defaultValue = 0.0f;
        protected override string KeyType => "float";
        protected override string DefaultValue => defaultValue.ToCode();
    }
}