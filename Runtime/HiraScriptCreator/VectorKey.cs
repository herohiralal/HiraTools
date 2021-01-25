namespace UnityEngine
{
    public class VectorKey : BlackboardKey
    {
        [SerializeField] private Vector3 defaultValue = Vector3.zero;
        protected override string KeyType => "UnityEngine.Vector3";

        protected override string DefaultValue =>
            $"new Vector3({defaultValue.x}f, {defaultValue.y}f, {defaultValue.z}f)";

        public override string ConstructorArgument =>
            $"{KeyType}? in{name} = null";

        public override string Initializer =>
            $"{name} = in{name} ?? {DefaultValue};";
    }
}