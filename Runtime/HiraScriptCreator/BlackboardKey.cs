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
                var validDefault = string.IsNullOrWhiteSpace(defaultValue) ? "default" : defaultValue;
                var serialize = unitySerialization ? "[SerializeField]" : "";
                return sb
                    .AppendLine($"    {serialize} private {keyType} {privateFormattedName} = {validDefault};")
                    .AppendLine($"    public {keyType} {name}")
                    .AppendLine(@"    {")
                    .AppendLine($"        get => {privateFormattedName};")
                    .AppendLine(@"        set")
                    .AppendLine(@"        {")
                    .AppendLine(@"            RaiseValueUpdateEvent();")
                    .AppendLine($"            {privateFormattedName} = value;")
                    .AppendLine(@"        }")
                    .AppendLine(@"    }")
                    .AppendLine(@"    ")
                    .ToString();
            }
        }

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