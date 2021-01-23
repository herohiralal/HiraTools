using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
	[System.Serializable]
	internal struct ValueAccessorInfo
	{
		public string typeName;
		public string niceName;
	}
	
	[CreateAssetMenu]
	public class Blackboard : HiraCollection<BlackboardKey>, IHiraScriptCreator
	{
		[SerializeField] private Blackboard parent = null;
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
				foreach (var blackboardKey in blackboard.collection1)
					sb.AppendLine($"        {blackboardKey.Initializer}");

				sb
					.AppendLine(@"    }")
					.AppendLine(@"    ");
				
				foreach (var blackboard in hierarchy)
				foreach (var blackboardKey in blackboard.collection1)
					sb.Append(blackboardKey.Code);

				// foreach (var blackboard in hierarchy)
				// {
				// 	foreach (var accessorInfo in blackboard.valueAccessorInfo)
				// 	{
				// 		blackboard.AppendGetter(sb, in accessorInfo);
				// 		blackboard.AppendSetter(sb, in accessorInfo);
				// 	}
				// }
				
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
			foreach (var blackboardKey in blackboard.collection1)
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
			foreach (var blackboardKey in blackboard.collection1)
			{
				var setter = blackboardKey.AppendSetter(accessorInfo.typeName);
				if (setter != null) sb.AppendLine($"        {setter}");
			}

			sb
				.AppendLine($"        Debug.LogError($\"Key not recognized: {{keyName}}. Blackboard: {name}.\");")
				.AppendLine(@"    }")
				.AppendLine(@"    ");
		}
	}
}