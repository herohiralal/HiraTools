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
	public class Blackboard : HiraCollection<IBlackboardKey, IBlackboardGoal, IBlackboardAction>
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
		, IHiraScriptCreator
#endif
	{
#pragma warning disable 414
		private static readonly string collection1_name = "Blackboard Values";
		private static readonly string collection2_name = "Goals";
		private static readonly string collection3_name = "Actions";
#pragma warning restore 414
		
		[SerializeField] private ScriptableObject[] dependencies = { };
		public string @namespace = "UnityEngine";
		[SerializeField] [HideInInspector] private string cachedFilePath = "";

		[SerializeField] private ValueAccessorInfo[] valueAccessorInfo =
		{
			new ValueAccessorInfo {niceName = "Boolean", typeName = "bool"},
			new ValueAccessorInfo {niceName = "Integer", typeName = "int"},
			new ValueAccessorInfo {niceName = "Float", typeName = "float"},
		};

#if UNITY_EDITOR && !STRIP_EDITOR_CODE
		public ScriptableObject[] Dependencies => dependencies;

		public string CachedFilePath
		{
			get => cachedFilePath;
			set => cachedFilePath = value;
		}

		public string FileName => name;

		public string FileData =>
			new StringBuilder(30000)
				.AppendLine(@"// ReSharper disable All") // it's a generated file, obviously not state of the art
				.AppendLine(@"")
				.AppendLine($"namespace {@namespace}")
				.AppendLine(@"{")
				.AppendLine(@"    [System.Serializable]")
				.AppendLine(@"    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]")
				.AppendLine($"    public struct {name}")
				.AppendLine(@"    {")
				.AppendLine(GetConstructor(name, false))
				.AppendLine()
				.Append(StructFields)
				.AppendLine(@"    }")
				.AppendLine(@"    ")
				.AppendLine(@"    [System.Serializable]")
				.AppendLine($"    public class {name}Wrapper")
				.AppendLine(@"    {")
				.AppendLine(GetConstructor($"{name}Wrapper", true))
				.AppendLine()
				.AppendLine(@"        public event System.Action OnValueUpdate = delegate { };")
				.AppendLine($"        [SerializeField] private {name} blackboard;")
				.Append(ClassProperties)
				.Append(Accessors)
				.AppendLine(@"    }")
				.AppendLine(@"    ")
				.AppendLine(@"    [Unity.Burst.BurstCompile]")
				.AppendLine($"    public readonly struct {name}ActionData")
				.AppendLine(@"    {")
				.AppendLine(@"        [Unity.Collections.ReadOnly] public readonly int Identifier;")
				.AppendLine(@"        [Unity.Collections.ReadOnly] public readonly int ArchetypeIndex;")
				.AppendLine(@"        [Unity.Collections.ReadOnly] public readonly float Cost;")
				.AppendLine(@"        ")
				.AppendLine($"        public {name}ActionData(int identifier, int archetypeIndex, float cost)")
				.AppendLine(@"        {")
				.AppendLine(@"            Identifier = identifier;")
				.AppendLine(@"            ArchetypeIndex = archetypeIndex;")
				.AppendLine(@"            Cost = cost;")
				.AppendLine(@"        }")
				.AppendLine(@"    }")
				.AppendLine(@"    ")
				.AppendLine($"    public static class {name}ArchetypeIndices")
				.AppendLine(@"    {")
				.Append(ArchetypeIndices)
				.AppendLine(@"    }")
				.AppendLine(@"    ")
				.AppendLine(@"    [Unity.Burst.BurstCompile]")
				.AppendLine($"    public struct {name}PlannerJob : Unity.Jobs.IJob")
				.AppendLine(@"    {")
				.AppendLine($"        [Unity.Collections.DeallocateOnJobCompletion] private Unity.Collections.NativeArray<{name}> _datasets;")
				.AppendLine(@"        [Unity.Collections.ReadOnly] private readonly int _goal;")
				.AppendLine($"        [Unity.Collections.ReadOnly] private readonly Unity.Collections.NativeArray<{name}ActionData> _actions;")
				.AppendLine(@"        [Unity.Collections.ReadOnly] private readonly int _actionsCount;")
				.AppendLine(@"        [Unity.Collections.ReadOnly] private readonly float _maxFScore;")
				.AppendLine(@"        [Unity.Collections.WriteOnly] public Unity.Collections.NativeArray<int> Plan;")
				.AppendLine(@"        ")
				.AppendLine(@"        public void Execute()")
				.AppendLine(@"        {")
				.AppendLine(@"        }")
				.AppendLine(@"    }")
				.AppendLine(@"}")
				.ToString();

		private string GetConstructor(string typeName, bool isWrapper)
		{
			var sb = new StringBuilder(250);
			sb
				.AppendLine($"        public {typeName}(");

			var allKeys = Collection1;
			// constructor arguments
			for (var i = 0; i < allKeys.Length; i++)
			{
				var key = allKeys[i];
				sb.Append(key.ConstructorArgument);
				if (i < allKeys.Length - 1) sb.AppendLine(",");
			}

			sb
				.AppendLine(")")
				.AppendLine(@"        {");
				
			// initializers
			foreach (var key in allKeys)
			{
				if (isWrapper)
				{
					sb.AppendLine(key.ClassInitializer);
					if(key.InstanceSynced)
						sb.AppendLine(key.WrapperEventBinder);
				}
				else
					sb.AppendLine(key.StructInitializer);
			}
				
			sb
				.Append(@"        }"); // constructor over

			if (isWrapper)
			{
				sb
					.AppendLine(@"")
					.AppendLine(@"")
					.AppendLine($"        ~{typeName}()")
					.AppendLine(@"        {");

				foreach (var key in allKeys)
				{
					if(key.InstanceSynced)
						sb.AppendLine(key.WrapperEventUnbinder);
				}
				
				sb
					.Append(@"        }");
			}
			
			return sb.ToString();
		}

		private string StructFields
		{
			get
			{
				var sb = new StringBuilder(500);
				foreach (var key in Collection1)
				{
					sb.AppendLine(key.StructField);
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

		private string ArchetypeIndices
		{
			get
			{
				int i;
				var s = "";
				s += "        public const int GOAL_UNINITIALIZED = 0;\n";
				for (i = 0; i < Collection2.Length; i++)
				{
					s += $"        public const int GOAL_{Collection2[i].Name.PascalToAllUpper()} = {i + 1};\n";
				}

				s += $"        public const int GOAL_COUNT = {i};\n";
				s += "        public const int ACTION_UNINITIALIZED = 0;\n";
				
				for (i = 0; i < Collection3.Length; i++)
				{
					s += $"        public const int ACTION_{Collection3[i].Name.PascalToAllUpper()} = {i + 1};\n";
				}

				s += $"        public const int ACTION_COUNT = {i};\n";
				
				return s;
			}
		}
#endif
	}
}