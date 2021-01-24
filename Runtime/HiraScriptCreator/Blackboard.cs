using System.Collections.Generic;
using System.Linq;
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
		[SerializeField] private Blackboard parent = null;

#if UNITY_EDITOR && !STRIP_EDITOR_CODE
		[SerializeField] private string @namespace = "UnityEngine";
		[SerializeField] [HideInInspector] private string cachedFilePath = "";

		[SerializeField] private ValueAccessorInfo[] valueAccessorInfo =
		{
			new ValueAccessorInfo {niceName = "Boolean", typeName = "bool"},
			new ValueAccessorInfo {niceName = "Integer", typeName = "int"},
			new ValueAccessorInfo {niceName = "Float", typeName = "float"},
			new ValueAccessorInfo {niceName = "String", typeName = "string"},
		};

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
				IBlackboardKey[] allKeys;
				{
					var hierarchy = new List<Blackboard> {this};
					var currentBlackboard = this;
					while ((currentBlackboard = currentBlackboard.parent) != null)
						hierarchy.Add(currentBlackboard);
					var blackboardCount = hierarchy.Count;
					allKeys = hierarchy.SelectMany(bb => bb.Collection1).ToArray();
				}
				if (allKeys.Length == 0) return "";

				var sb = new StringBuilder(5000);
				sb
					.AppendLine(@"using System;")
					.AppendLine(@"using UnityEngine;")
					.AppendLine(@"using System.Runtime.InteropServices;")
					.AppendLine(@"// ReSharper disable All") // it's a generated file, obviously not state of the art
					.AppendLine(@"")
					.AppendLine($"namespace {@namespace}")
					.AppendLine(@"{")
					.AppendLine(@"    [Serializable]")
					.AppendLine(@"    [StructLayout(LayoutKind.Sequential)]")
					.AppendLine($"    public struct {name}")
					.AppendLine(@"    {")
					.AppendLine(GetConstructor(name, "this", allKeys))
					.AppendLine()
					.AppendLine(GetStructFields(allKeys))
					.AppendLine(@"    }")
					.AppendLine(@"    ")
					.AppendLine(@"    [Serializable]")
					.AppendLine($"    public class {name}Wrapper")
					.AppendLine(@"    {")
					.AppendLine(GetConstructor($"{name}Wrapper", "blackboard", allKeys))
					.AppendLine()
					.AppendLine(@"        public event Action OnValueUpdate = delegate { };")
					.AppendLine($"        [SerializeField] private {name} blackboard;")
					.Append(GetClassProperties(allKeys))
					.Append(GetAccessors(allKeys))
					.AppendLine(@"    }")
					.AppendLine(@"}"); // namespace over
				return sb.ToString();
			}
		}

		private static string GetConstructor(string typeName, string variableName, IBlackboardKey[] allKeys)
		{
			var sb = new StringBuilder(250);
			sb
				.Append($"        public {typeName}(");
				
			// constructor arguments
			for (var i = 0; i < allKeys.Length; i++)
			{
				var key = allKeys[i];
				sb.Append(key.ConstructorArgument);
				if (i < allKeys.Length - 1) sb.Append(", ");
			}

			sb
				.AppendLine(")")
				.AppendLine(@"        {");
				
			// initializers
			foreach (var key in allKeys)
			{
				sb.AppendLine($"            {variableName}.{key.Initializer}");
			}
				
			sb
				.Append(@"        }"); // constructor over
			return sb.ToString();
		}

		private static string GetStructFields(IBlackboardKey[] allKeys)
		{
			var sb = new StringBuilder(500);
			foreach (var key in allKeys)
			{
				sb.AppendLine($"        {key.StructField}");
			}

			return sb.ToString();
		}

		private static string GetClassProperties(IBlackboardKey[] allKeys)
		{
			var sb = new StringBuilder(500);
			foreach (var key in allKeys)
			{
				sb.AppendLine(key.ClassProperty);
			}

			return sb.ToString();
		}

		private string GetAccessors(IBlackboardKey[] allKeys)
		{
			var sb = new StringBuilder(1000);
			foreach (var accessorInfo in valueAccessorInfo)
			{
				sb
					.Append(GetGetter(allKeys, in accessorInfo))
					.Append(GetSetter(allKeys, in accessorInfo));
			}

			return sb.ToString();
		}

		private string GetGetter(IBlackboardKey[] allKeys, in ValueAccessorInfo accessorInfo)
		{
			var sb = new StringBuilder(500);
			sb
				.AppendLine(@"        ")
				.AppendLine($"        public {accessorInfo.typeName} Get{accessorInfo.niceName}Value(string keyName)")
				.AppendLine(@"        {");
			
			foreach (var key in allKeys)
			{
				var getter = key.GetGetter(accessorInfo.typeName);
				if (getter != null) sb.AppendLine($"            {getter}");
			}

			sb
				.AppendLine($"            Debug.LogError($\"Key not recognized: {{keyName}}. Blackboard: {name}.\");")
				.AppendLine(@"            return default;")
				.AppendLine(@"        }");
			
			return sb.ToString();
		}

		private string GetSetter(IBlackboardKey[] allKeys, in ValueAccessorInfo accessorInfo)
		{
			var sb = new StringBuilder(500);
			sb
				.AppendLine(@"        ")
				.AppendLine($"        public void Set{accessorInfo.niceName}Value(string keyName, {accessorInfo.typeName} newValue)")
				.AppendLine(@"        {");
			foreach (var key in allKeys)
			{
				var setter = key.GetSetter(accessorInfo.typeName);
				if (setter != null) sb.AppendLine($"            {setter}");
			}

			sb
				.AppendLine($"            Debug.LogError($\"Key not recognized: {{keyName}}. Blackboard: {name}.\");")
				.AppendLine(@"        }");

			return sb.ToString();
		}
		//
		// private void AppendSetter(StringBuilder sb, in ValueAccessorInfo accessorInfo, List<Blackboard> blackboards)
		// {
		// 	sb
		// 		.AppendLine(
		// 			$"    public void Set{accessorInfo.niceName}Value(string keyName, {accessorInfo.typeName} newValue)")
		// 		.AppendLine(@"    {");
		//
		// 	foreach (var blackboard in blackboards)
		// 	foreach (var blackboardKey in blackboard.Collection1)
		// 	{
		// 		var setter = blackboardKey.AppendSetter(accessorInfo.typeName);
		// 		if (setter != null) sb.AppendLine($"        {setter}");
		// 	}
		//
		// 	sb
		// 		.AppendLine($"        Debug.LogError($\"Key not recognized: {{keyName}}. Blackboard: {name}.\");")
		// 		.AppendLine(@"    }")
		// 		.AppendLine(@"    ");
		// }
#endif
	}
}