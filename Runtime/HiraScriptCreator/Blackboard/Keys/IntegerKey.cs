namespace UnityEngine
{
    public class IntegerKey : BlackboardKey, INumericalKey
    {
        [SerializeField] private int defaultValue = 0;
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        protected override string KeyType => "int";
        protected override string DefaultValue => defaultValue.ToCode();
#endif
    }
}