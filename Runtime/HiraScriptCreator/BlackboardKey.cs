namespace UnityEngine
{
    public interface IBlackboardKey
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        string StructField { get; }
        string ClassProperty { get; }
        string ConstructorArgument { get; }
        string Initializer { get; }
        string GetGetter(string type);
        string GetSetter(string type);
#endif
    }

    public abstract class BlackboardKey : ScriptableObject, IBlackboardKey
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        protected abstract string KeyType { get; }
        protected abstract string DefaultValue { get; }

        public virtual string StructField => $"public {KeyType} {name};";

        public virtual string ClassProperty =>
            $"        \n" +
            $"        public {KeyType} {name}\n" +
            $"        {{\n" +
            $"            get => blackboard.{name};\n" +
            $"            set\n" +
            $"            {{\n" +
            $"                OnValueUpdate.Invoke();\n" +
            $"                blackboard.{name} = value;\n" +
            $"            }}\n" +
            $"        }}";

        public virtual string ConstructorArgument =>
            $"{KeyType} in{name} = {(string.IsNullOrWhiteSpace(DefaultValue) ? "default" : DefaultValue)}";

        public virtual string Initializer =>
            $"{name} = in{name};";

        public virtual string GetGetter(string type) =>
            type == KeyType
                ? $"                case \"{name}\":\n" +
                  $"                    return {name};"
                : null;

        public virtual string GetSetter(string type) =>
            type == KeyType 
                ? $"                case \"{name}\":\n" +
                  $"                    {name} = newValue;\n" +
                  $"                    return;" 
                : null;
#endif
    }
}