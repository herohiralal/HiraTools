namespace UnityEngine
{
    public class BooleanKey : BlackboardKey
    {
        [SerializeField] private bool defaultValue = false;
        protected override string KeyType => "bool";
        protected override string DefaultValue => defaultValue.ToString().ToLowerInvariant();
    }
}