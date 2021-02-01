namespace UnityEngine
{
    public class VectorKey : BlackboardKey
    {
        [SerializeField] private Vector3 defaultValue = Vector3.zero;
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        protected override string KeyType => "UnityEngine.Vector3";

        protected override string DefaultValue => defaultValue.ToCode();
#endif
    }
}