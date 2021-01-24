using System.Text;

namespace UnityEngine
{
    public interface IBlackboardKey
    {
        string StructField { get; }
        string ClassProperty { get; }
        string ConstructorArgument { get; }
        string Initializer { get; }
        string GetGetter(string type);
        string GetSetter(string type);
    }
    
    public class BlackboardKey : ScriptableObject
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        , IBlackboardKey
#endif
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        [StringDropdown(true, "bool", "float", "int", "string")]
        [SerializeField] private string keyType = "bool";
        [SerializeField] private string defaultValue = "default";
        [SerializeField] private bool unitySerialization = true;

        public string StructField => $"public {keyType} {name};";

        public string ClassProperty =>
            $"        \n" +
            $"        public {keyType} {name}\n" +
            $"        {{\n" +
            $"            get => blackboard.{name};\n" +
            $"            set\n" +
            $"            {{\n" +
            $"                OnValueUpdate.Invoke();\n" +
            $"                blackboard.{name} = value;\n" +
            $"            }}\n" +
            $"        }}";

        public string ConstructorArgument =>
            $"{keyType} in{name} = {(string.IsNullOrWhiteSpace(defaultValue) ? "default" : defaultValue)}";

        public string Initializer =>
            $"{name} = in{name};";

        public string GetGetter(string type) =>
            type == keyType ? $"if (keyName == \"{name}\") return {name};" : null;

        public string GetSetter(string type) =>
            type == keyType ? $"if (keyName == \"{name}\") {{ {name} = newValue; return; }}" : null;
#endif
    }
}