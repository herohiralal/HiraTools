namespace UnityEngine
{
    public abstract class EnumKey<T> : BlackboardKey where T : System.Enum
    {
        [SerializeField] protected T defaultValue = default;
        protected override string KeyType => typeof(T).FullName;
        protected override string DefaultValue => defaultValue.ToCode();

        public override string StructField => base.StructField + GetBooleanAccessors(false);
        public override string ClassProperty => base.ClassProperty + GetBooleanAccessors(true);

        protected virtual string GetBooleanAccessors(bool isWrapper)
        {
            var s = "";
            var stat = instanceSynced && isWrapper ? "static " : "";
            var typeString = typeof(T).FullName;
            var enumNames = System.Enum.GetNames(typeof(T));
            var namesCount = enumNames.Length;
            for (var i = 0; i < namesCount; i++)
            {
                var enumName = enumNames[i];
                s +=
                    $"\n" +
                    $"        \n" +
                    $"        public {stat}bool {name}{enumName}\n" +
                    $"        {{\n" +
                    $"            get => (({name} & {typeString}.{enumName}) == {typeString}.{enumName});\n" +
                    $"            set\n" +
                    $"            {{\n" +
                    $"                if (value) {name} |= {typeString}.{enumName};\n" +
                    $"                else {name} &= ~{typeString}.{enumName};\n" +
                    $"            }}\n" +
                    $"        }}";
            }

            return s;
        }

        public override string GetGetter(string type)
        {
            if (type == KeyType)
                return base.GetGetter(type);
            
            switch (type)
            {
                case "bool":
                {
                    var enumNames = System.Enum.GetNames(typeof(T));
                    var namesCount = enumNames.Length;
                    var s = "";
                    for (var i = 0; i < namesCount; i++)
                    {
                        var enumName = enumNames[i];
                        s +=
                            $"                case \"{name}{enumName}\":\n" +
                            $"                    return {name}{enumName};";
                    
                        if (i != namesCount - 1) s += '\n';
                    }

                    return s;
                }
                case "int":
                    return
                        $"                case \"{name}\":\n" +
                        $"                    return (int) {name};";
                default:
                    return null;
            }
        }

        public override string GetSetter(string type)
        {
            if (type == KeyType)
                return base.GetSetter(type);
            
            switch (type)
            {
                case "bool":
                {
                    var enumNames = System.Enum.GetNames(typeof(T));
                    var namesCount = enumNames.Length;
                    var s = "";
                    for (var i = 0; i < namesCount; i++)
                    {
                        var enumName = enumNames[i];
                        s +=
                            $"                case \"{name}{enumName}\":\n" +
                            $"                    {name}{enumName} = newValue;\n" +
                            $"                    return;";
                    
                        if (i != namesCount - 1) s += '\n';
                    }

                    return s;
                }
                case "int":
                    return $"                case \"{name}\":\n" +
                           $"                    {name} = ({KeyType}) newValue;\n" +
                           @"                    return;";
                default:
                    return null;
            }
        }
    }
}