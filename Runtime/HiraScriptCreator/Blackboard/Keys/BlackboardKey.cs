namespace UnityEngine
{
    public interface IBlackboardKey
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        string StructField { get; }
        string ClassProperty { get; }
        string ConstructorArgument { get; }
        string StructInitializer { get; }
        string ClassInitializer { get; }
        string WrapperEventBinder { get; }
        string WrapperEventUnbinder { get; }
        bool InstanceSynced { get; }
        string GetGetter(string type);
        string GetSetter(string type);
#endif
    }

    public abstract class BlackboardKey : ScriptableObject, IBlackboardKey
    {
        [SerializeField] protected bool instanceSynced = false;
        public bool InstanceSynced => instanceSynced;
        
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        protected abstract string KeyType { get; }
        protected abstract string DefaultValue { get; }

        protected virtual string DefaultValueInternal =>
            (string.IsNullOrWhiteSpace(DefaultValue) ? "default" : DefaultValue);

        public virtual string StructField => $"        public {KeyType} {name};";
        public virtual string ClassProperty
        {
            get
            {
                if (instanceSynced)
                {
                    var staticFieldName = name.PascalToCamel();
                    var defaultValue = DefaultValueInternal;
                    return
                        $"        \n" +
                        $"        private static {KeyType} {staticFieldName} = {defaultValue};\n" +
                        $"        private static event System.Action On{name}Change = delegate {{ }};\n" +
                        $"        \n" +
                        $"        private void On{name}Updated()\n" +
                        $"        {{\n" +
                        $"            blackboard.{name} = {staticFieldName};\n" +
                        $"            OnValueUpdate.Invoke();\n" +
                        $"        }}\n" +
                        $"        \n" +
                        $"        public static {KeyType} {name}\n" +
                        $"        {{\n" +
                        $"            get => {staticFieldName};\n" +
                        $"            set\n" +
                        $"            {{\n" +
                        $"                {staticFieldName} = value;\n" +
                        $"                On{name}Change.Invoke();\n" +
                        $"            }}\n" +
                        $"        }}";
                }
                else
                {
                    return
                        $"        \n" +
                        $"        public {KeyType} {name}\n" +
                        $"        {{\n" +
                        $"            get => blackboard.{name};\n" +
                        $"            set\n" +
                        $"            {{\n" +
                        $"                blackboard.{name} = value;\n" +
                        $"                OnValueUpdate.Invoke();\n" +
                        $"            }}\n" +
                        $"        }}";
                }
            }
        }

        public virtual string ConstructorArgument =>
            $"            {KeyType}? in{name} = null";

        public virtual string StructInitializer =>
            $"            {name} = in{name} ?? {DefaultValueInternal};";

        public string ClassInitializer
        {
            get
            {
                if (instanceSynced)
                {
                    var staticFieldName = name.PascalToCamel();
                    return
                        $"            if (in{name}.HasValue)\n" +
                        $"            {{\n" +
                        $"                {staticFieldName} = in{name}.Value;\n" +
                        $"                On{name}Change.Invoke();\n" +
                        $"            }}\n" +
                        $"            blackboard.{name} = {staticFieldName};";
                }
                else
                {
                    var defaultValueInternal = DefaultValueInternal;
                    return
                        $"            blackboard.{name} = in{name} ?? {defaultValueInternal};";
                }
            }
        }

        public string WrapperEventBinder =>
            $"            On{name}Change += On{name}Updated;";
        
        public string WrapperEventUnbinder =>
            $"            On{name}Change -= On{name}Updated;";

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