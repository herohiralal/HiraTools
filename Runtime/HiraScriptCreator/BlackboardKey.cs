using System.Text;

namespace UnityEngine
{
    public class BlackboardKey : ScriptableObject
    {
        [StringDropdown(true, "bool", "float", "int", "string")]
        [SerializeField] private string keyType = "bool";
        [SerializeField] private string defaultValue = "default";
        [SerializeField] private bool unitySerialization = true;

        public string Code
        {
            get
            {
                var sb = new StringBuilder(100);
                var privateFormattedName = PrivateFormattedName;
                var serialize = unitySerialization ? "[SerializeField]" : "";
                return sb
                    .AppendLine($"    {serialize} private {keyType} {privateFormattedName};")
                    .AppendLine($"    public {keyType} {name}")
                    .AppendLine(@"    {")
                    .AppendLine($"        get => {privateFormattedName};")
                    .AppendLine(@"        set")
                    .AppendLine(@"        {")
                    .AppendLine(@"            OnValueUpdate.Invoke();")
                    .AppendLine($"            {privateFormattedName} = value;")
                    .AppendLine(@"        }")
                    .AppendLine(@"    }")
                    .AppendLine(@"    ")
                    .ToString();
            }
        }

        public string Initializer =>
            $"{PrivateFormattedName} = {(string.IsNullOrWhiteSpace(defaultValue) ? "default" : defaultValue)};";

        public string AppendGetter(string type) =>
            type == keyType ? $"if (keyName == \"{name}\") return {name};" : null;

        public string AppendSetter(string type) =>
            type == keyType ? $"if (keyName == \"{name}\") {{ {name} = newValue; return; }}" : null;

        private string PrivateFormattedName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name)) return "";

                var characters = name.ToCharArray();
                characters[0] = char.ToLower(characters[0]);
                var formattedName = new string(characters);
                return unitySerialization 
                    ? formattedName 
                    : "_" + formattedName;
            }
        }
    }
}