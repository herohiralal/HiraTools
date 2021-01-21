using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine
{
	[CreateAssetMenu]
	public class Blackboard : ScriptableObject, IHiraScriptCreator
	{
		[SerializeField] private Blackboard parent = null;
		[SerializeField] private KeyData[] keyData = null;
		[SerializeField] [HideInInspector] private string cachedFilePath = "";
		[SerializeField] [HideInInspector] private Object[] dependencies = new Object[0];

		[Serializable]
		private struct KeyData
		{
			public string keyName;

			[StringDropdown(false, "bool", "float", "int", "string")]
			public string type;
		}

		public bool HasDependencies => dependencies.Length > 0;
		public IEnumerable<Object> Dependencies => dependencies;
		public string CachedFilePath
		{
			get => cachedFilePath;
			set => cachedFilePath = value;
		}
		public string FileName => name;
		public string FileData
		{
			get
			{
				var sb = new StringBuilder(5000);
				sb
					.AppendLine(@"using System;")
					.AppendLine(@"");

				if (parent == null)
				{
					sb
						.AppendLine($"public class {name}")
						.AppendLine(@"{")
						.AppendLine(@"    public event Action OnValueUpdate = delegate { };")
						.AppendLine(@"    ")
						.AppendLine(@"    protected void RaiseValueUpdateEvent() => OnValueUpdate.Invoke();");
				}
				else
				{
					sb.AppendLine($"public class {name} : {parent.name}")
						.AppendLine(@"{");
				}

				foreach (var data in keyData)
				{
					var formattedKeyName = FormatToPrivateField(data.keyName);
					sb
						.AppendLine($"    private {data.type} {formattedKeyName} = default;")
						.AppendLine($"    public {data.type} {data.keyName}")
						.AppendLine(@"    {")
						.AppendLine($"        get => {formattedKeyName};")
						.AppendLine(@"        set")
						.AppendLine(@"        {")
						.AppendLine(@"            RaiseValueUpdateEvent();")
						.AppendLine($"            {formattedKeyName} = value;")
						.AppendLine(@"        }")
						.AppendLine(@"    }")
						.AppendLine(@"    ");
				}

				return sb.AppendLine(@"}")
					.ToString();
			}
		}

		private static string FormatToPrivateField(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return "";

			var characters = name.ToCharArray();
			characters[0] = char.ToLower(characters[0]);
			return "_" + new string(characters);
		}
	}
}