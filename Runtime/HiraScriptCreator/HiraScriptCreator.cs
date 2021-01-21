namespace UnityEngine
{
    public abstract class HiraScriptCreator : ScriptableObject
    {
        public abstract string FileName { get; }
        public abstract string FileData { get; }
    }
}

// [SerializeField] private KeyData[] keyData = null;
//
// [Serializable]
// private struct KeyData
// {
//     public string keyName;
//
//     [StringDropdown(false, "bool", "float", "int", "string")]
//     public string type;
// }
//
// public string GenerateFileData()
// {
//     var sb = new StringBuilder(500);
//     sb
//         .AppendLine(@"using System;")
//         .AppendLine(@"")
//         .AppendLine($"public class {name}Blackboard")
//         .AppendLine(@"{")
//         .AppendLine(@"    public event Action OnValueUpdate = delegate { };")
//         .AppendLine(@"    ");
//
//     foreach (var data in keyData)
//     {
//         var formattedKeyName = FormatToPrivateField(data.keyName);
//         sb
//             .AppendLine($"    private {data.type} {formattedKeyName} = default;")
//             .AppendLine($"    public {data.type} {data.keyName}")
//             .AppendLine(@"    {")
//             .AppendLine($"        get => {formattedKeyName};")
//             .AppendLine(@"        set")
//             .AppendLine(@"        {")
//             .AppendLine(@"            OnValueUpdate.Invoke();")
//             .AppendLine($"            {formattedKeyName} = value;")
//             .AppendLine(@"        }")
//             .AppendLine(@"    }")
//             .AppendLine(@"    ");
//     }
//
//     return sb.AppendLine(@"}")
//         .ToString();
// }
//
// private static string FormatToPrivateField(string name)
// {
//     if (string.IsNullOrWhiteSpace(name)) return "";
//
//     var characters = name.ToCharArray();
//     characters[0] = char.ToLower(characters[0]);
//     return "_" + new string(characters);
// }