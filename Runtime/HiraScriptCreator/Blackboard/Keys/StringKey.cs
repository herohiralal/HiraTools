namespace UnityEngine
{
    public class StringKey : BlackboardKey
    {
        [SerializeField] private string defaultValue = "";
        
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        protected override string KeyType => "string";
        protected override string DefaultValue => defaultValue.ToCode();
#endif
    }
}