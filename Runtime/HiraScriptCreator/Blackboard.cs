using System.Collections.Generic;
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
	public class Blackboard : HiraCollection<BlackboardKey>
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
		, IHiraScriptCreator
#endif
	{
		[SerializeField] private Blackboard parent = null;
		
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
		[SerializeField] [HideInInspector] private string cachedFilePath = "";

		[SerializeField] private ValueAccessorInfo[] valueAccessorInfo =
		{
			new ValueAccessorInfo{niceName = "Boolean", typeName = "bool"},
			new ValueAccessorInfo{niceName = "Integer", typeName = "int"},
			new ValueAccessorInfo{niceName = "Float", typeName = "float"},
			new ValueAccessorInfo{niceName = "String", typeName = "string"},
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
				var hierarchy = new List<Blackboard> {this};
				var currentBlackboard = this;
				while ((currentBlackboard = currentBlackboard.parent) != null)
					hierarchy.Add(currentBlackboard);

				var sb = new StringBuilder(5000);
				sb
					.AppendLine(@"using System;")
					.AppendLine(@"using UnityEngine;")
					.AppendLine(@"using System.Runtime.InteropServices;")
					.AppendLine(@"// ReSharper disable All") // it's a generated file, obviously it's not state of the art
					.AppendLine(@"")
					.AppendLine(@"[Serializable]")
					.AppendLine(@"[StructLayout(LayoutKind.Sequential)]")
					.AppendLine($"public struct {name}")
					.AppendLine(@"{")
					.AppendLine(@"    public event Action OnValueUpdate;")
					.AppendLine(@"    ")
					.AppendLine($"    public {name}(bool _)")
					.AppendLine(@"    {")
					.AppendLine(@"        OnValueUpdate = delegate { };")
					.AppendLine(@"        ");
				
				foreach (var blackboard in hierarchy)
				foreach (var blackboardKey in blackboard.Collection1)
					sb.AppendLine($"        {blackboardKey.Initializer}");

				sb
					.AppendLine(@"    }")
					.AppendLine(@"    ");
				
				foreach (var blackboard in hierarchy)
				foreach (var blackboardKey in blackboard.Collection1)
					sb.Append(blackboardKey.Code);
				
				foreach (var accessorInfo in valueAccessorInfo)
				{
					AppendGetter(sb, in accessorInfo, hierarchy);
					AppendSetter(sb, in accessorInfo, hierarchy);
				}

				return sb.AppendLine(@"}")
					.ToString();
			}
		}

		private void AppendGetter(StringBuilder sb, in ValueAccessorInfo accessorInfo, List<Blackboard> blackboards)
		{
			sb
				.AppendLine($"    public {accessorInfo.typeName} Get{accessorInfo.niceName}Value(string keyName)")
				.AppendLine(@"    {");
			
			foreach (var blackboard in blackboards)
			foreach (var blackboardKey in blackboard.Collection1)
			{
				var getter = blackboardKey.AppendGetter(accessorInfo.typeName);
				if (getter != null) sb.AppendLine($"        {getter}");
			}

			sb
				.AppendLine($"        Debug.LogError($\"Key not recognized: {{keyName}}. Blackboard: {name}.\");")
				.AppendLine(@"        return default;")
				.AppendLine(@"    }")
				.AppendLine(@"    ");
		}

		private void AppendSetter(StringBuilder sb, in ValueAccessorInfo accessorInfo, List<Blackboard> blackboards)
		{
			sb
				.AppendLine($"    public void Set{accessorInfo.niceName}Value(string keyName, {accessorInfo.typeName} newValue)")
				.AppendLine(@"    {");
			
			foreach (var blackboard in blackboards)
			foreach (var blackboardKey in blackboard.Collection1)
			{
				var setter = blackboardKey.AppendSetter(accessorInfo.typeName);
				if (setter != null) sb.AppendLine($"        {setter}");
			}

			sb
				.AppendLine($"        Debug.LogError($\"Key not recognized: {{keyName}}. Blackboard: {name}.\");")
				.AppendLine(@"    }")
				.AppendLine(@"    ");
		}
#endif
	}
}