namespace UnityEngine
{
    public class FloatKey : BlackboardKey, INumericalKey
    {
        [SerializeField] private float defaultValue = 0.0f;
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        protected override string KeyType => "float";
        protected override string DefaultValue => defaultValue.ToCode();
#endif
    }
}