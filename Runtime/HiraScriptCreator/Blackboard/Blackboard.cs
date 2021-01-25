using System.Text;

namespace UnityEngine
{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
	[System.Serializable]
	internal struct ValueAccessorInfo
	{
		public string typeName;
		public string niceName;
	}
#endif

	[CreateAssetMenu]
	public class Blackboard : HiraCollection<IBlackboardKey>
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
		, IHiraScriptCreator
#endif
	{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
		// [SerializeField] private Blackboard parent = null;
		[SerializeField] private ScriptableObject[] dependencies = { };
		public string @namespace = "UnityEngine";
		[SerializeField] [HideInInspector] private string cachedFilePath = "";

		[SerializeField] private ValueAccessorInfo[] valueAccessorInfo =
		{
			new ValueAccessorInfo {niceName = "Boolean", typeName = "bool"},
			new ValueAccessorInfo {niceName = "Integer", typeName = "int"},
			new ValueAccessorInfo {niceName = "Float", typeName = "float"},
		};

		public ScriptableObject[] Dependencies => dependencies;

		public string CachedFilePath
		{
			get => cachedFilePath;
			set => cachedFilePath = value;
		}

		public string FileName => name;

		public string FileData =>
			new StringBuilder(5000)
				.AppendLine(@"// ReSharper disable All") // it's a generated file, obviously not state of the art
				.AppendLine(@"")
				.AppendLine($"namespace {@namespace}")
				.AppendLine(@"{")
				.AppendLine(@"    [System.Serializable]")
				.AppendLine(@"    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]")
				.AppendLine($"    public struct {name}")
				.AppendLine(@"    {")
				.AppendLine(GetConstructor(name, ""))
				.AppendLine()
				.AppendLine(StructFields)
				.AppendLine(@"    }")
				.AppendLine(@"    ")
				.AppendLine(@"    [System.Serializable]")
				.AppendLine($"    public class {name}Wrapper")
				.AppendLine(@"    {")
				.AppendLine(GetConstructor($"{name}Wrapper", "blackboard."))
				.AppendLine()
				.AppendLine(@"        public event System.Action OnValueUpdate = delegate { };")
				.AppendLine($"        [SerializeField] private {name} blackboard;")
				.Append(ClassProperties)
				.Append(Accessors)
				.AppendLine(@"    }")
				.AppendLine(@"}")
				.ToString();

		private string GetConstructor(string typeName, string variableName)
		{
			var sb = new StringBuilder(250);
			sb
				.AppendLine($"        public {typeName}(");

			var allKeys = Collection1;
			// constructor arguments
			for (var i = 0; i < allKeys.Length; i++)
			{
				var key = allKeys[i];
				sb.Append($"            {key.ConstructorArgument}");
				if (i < allKeys.Length - 1) sb.AppendLine(",");
			}

			sb
				.AppendLine(")")
				.AppendLine(@"        {");
				
			// initializers
			foreach (var key in allKeys)
			{
				sb.AppendLine($"            {variableName}{key.Initializer}");
			}
				
			sb
				.Append(@"        }"); // constructor over
			return sb.ToString();
		}

		private string StructFields
		{
			get
			{
				var sb = new StringBuilder(500);
				foreach (var key in Collection1)
				{
					sb.AppendLine($"        {key.StructField}");
				}

				return sb.ToString();
			}
		}

		private string ClassProperties
		{
			get
			{
				var sb = new StringBuilder(500);
				foreach (var key in Collection1)
				{
					sb.AppendLine(key.ClassProperty);
				}

				return sb.ToString();
			}
		}

		private string Accessors
		{
			get
			{
				var sb = new StringBuilder(1000);
				foreach (var accessorInfo in valueAccessorInfo)
				{
					sb
						.Append(GetGetter(in accessorInfo))
						.Append(GetSetter(in accessorInfo));
				}

				return sb.ToString();
			}
		}

		private string GetGetter(in ValueAccessorInfo accessorInfo)
		{
			var sb = new StringBuilder(500);
			sb
				.AppendLine(@"        ")
				.AppendLine($"        public {accessorInfo.typeName} Get{accessorInfo.niceName}Value(string keyName)")
				.AppendLine(@"        {")
				.AppendLine(@"            switch (keyName)")
				.AppendLine(@"            {");
			
			foreach (var key in Collection1)
			{
				var getter = key.GetGetter(accessorInfo.typeName);
				if (getter != null) sb.AppendLine(getter);
			}

			return sb
				.AppendLine(@"                default:")
				.AppendLine($"                    Debug.LogError($\"Key not recognized: {{keyName}}. Blackboard: {name}.\");")
				.AppendLine(@"                    return default;")
				.AppendLine(@"            }")
				.AppendLine(@"        }")
				.ToString();
		}

		private string GetSetter(in ValueAccessorInfo accessorInfo)
		{
			var sb = new StringBuilder(500);
			sb
				.AppendLine(@"        ")
				.AppendLine($"        public void Set{accessorInfo.niceName}Value(string keyName, {accessorInfo.typeName} newValue)")
				.AppendLine(@"        {")
				.AppendLine(@"            switch (keyName)")
				.AppendLine(@"            {");
			foreach (var key in Collection1)
			{
				var setter = key.GetSetter(accessorInfo.typeName);
				if (setter != null) sb.AppendLine(setter);
			}

			return sb
				.AppendLine(@"                default:")
				.AppendLine($"                    Debug.LogError($\"Key not recognized: {{keyName}}. Blackboard: {name}.\");")
				.AppendLine(@"                    return;")
				.AppendLine(@"            }")
				.AppendLine(@"        }")
				.ToString();
		}
#endif
	}
}