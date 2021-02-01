namespace UnityEngine
{
    public class BooleanKey : BlackboardKey
    {
        [SerializeField] private bool defaultValue = false;
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        protected override string KeyType => "bool";
        protected override string DefaultValue => defaultValue.ToCode();
#endif
    }
}