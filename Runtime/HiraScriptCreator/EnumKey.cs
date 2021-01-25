namespace UnityEngine
{
    public abstract class EnumKey<T> : BlackboardKey where T : System.Enum
    {
        [SerializeField] protected T defaultValue = default;
        protected override string KeyType => typeof(T).FullName;
        protected override string DefaultValue => $"{typeof(T).FullName}.{defaultValue}";

        public override string GetGetter(string type) =>
            type == KeyType
                ? base.GetGetter(type)
                : (type == "int"
                    ? $"                case \"{name}\":\n" +
                      $"                    return (int) {name};"
                    : null);

        public override string GetSetter(string type) =>
            type == KeyType
                ? base.GetSetter(type)
                : (type == "int"
                    ? $"                case \"{name}\":\n" +
                      $"                    {name} = ({KeyType}) newValue;\n" +
                      @"                    return;"
                    : null);
    }
}