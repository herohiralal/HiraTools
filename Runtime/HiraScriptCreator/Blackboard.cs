using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine
{
	[CreateAssetMenu]
	public class Blackboard : HiraCollection<BlackboardKey>, IHiraScriptCreator
	{
		[SerializeField] private Blackboard parent = null;
		[SerializeField] [HideInInspector] private string cachedFilePath = "";
		
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
					.AppendLine(@"using UnityEngine;")
					.AppendLine(@"")
					.AppendLine(@"[Serializable]");

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

				foreach (var data in collection1) 
					sb.Append(data.Code);

				return sb.AppendLine(@"}")
					.ToString();
			}
		}
	}
}