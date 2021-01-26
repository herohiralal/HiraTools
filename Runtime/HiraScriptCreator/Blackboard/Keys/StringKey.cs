namespace UnityEngine
{
    public class StringKey : BlackboardKey
    {
        [SerializeField] private string defaultValue = "";
        protected override string KeyType => "string";
        protected override string DefaultValue => defaultValue.ToCode();
    }
}