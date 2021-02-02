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
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
#pragma warning disable 414
		private static readonly string collection1_name = "Blackboard Values";
		private static readonly string collection2_name = "Goals";
		private static readonly string collection3_name = "Actions";
#pragma warning restore 414
#endif
		
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
				.AppendLine(@"    [Unity.Burst.BurstCompile]")
				.AppendLine($"    public struct {name} : UnityEngine.IBlackboard")
				.AppendLine(@"    {")
				.AppendLine(GetConstructor(name, false))
				.AppendLine()
				.Append(StructFields)
				.AppendLine(@"        ")
				.AppendLine(@"        [Unity.Burst.BurstCompile]")
				.AppendLine(@"        public static class ArchetypeIndices")
				.AppendLine(@"        {")
				.Append(ArchetypeIndices)
				.AppendLine(@"        }")
				.AppendLine(@"        ")
				.AppendLine(@"        [Unity.Burst.BurstCompile]")
				.AppendLine($"        public bool GetGoalValidity(int target) =>")
				.AppendLine(@"            target switch")
				.AppendLine(@"            {")
				.Append(GoalValidities)
				.AppendLine(@"            };")
				.AppendLine(@"        ")
				.AppendLine(@"        [Unity.Burst.BurstCompile]")
				.AppendLine($"        public int GetGoalHeuristic(int target) =>")
				.AppendLine(@"            target switch")
				.AppendLine(@"            {")
				.Append(GoalHeuristics)
				.AppendLine(@"            };")
				.AppendLine(@"        ")
				.AppendLine(@"        [Unity.Burst.BurstCompile]")
				.AppendLine($"        public bool GetActionApplicability(int target) =>")
				.AppendLine(@"            target switch")
				.AppendLine(@"            {")
				.Append(ActionPreconditions)
				.AppendLine(@"            };")
				.AppendLine(@"        ")
				.AppendLine(@"        [Unity.Burst.BurstCompile]")
				.AppendLine($"        public void ApplyActionEffect(int target)")
				.AppendLine(@"        {")
				.AppendLine(@"            switch (target)")
				.AppendLine(@"            {")
				.Append(ActionModifications)
				.AppendLine(@"            }")
				.AppendLine(@"        }")
				.AppendLine(@"    }")
				.AppendLine(@"    ")
				.AppendLine(@"    [System.Serializable]")
				.AppendLine($"    public class {name}Wrapper : UnityEngine.IBlackboard")
				.AppendLine(@"    {")
				.AppendLine(GetConstructor($"{name}Wrapper", true))
				.AppendLine(@"        ")
				.AppendLine($"        public {name}Wrapper()")
				.AppendLine(@"            : this")
				.AppendLine(@"            (")
				.AppendLine(Collection1.ConcatenateStringsWith(s=>"                null", ",\n"))
				.AppendLine(@"            )")
				.AppendLine(@"        {")
				.AppendLine(@"        }")
				.AppendLine(@"        ")
				.AppendLine(@"        public event System.Action OnValueUpdate = delegate { };")
				.AppendLine($"        [SerializeField] public {name} blackboard;")
				.Append(ClassProperties)
				.Append(Accessors)
				.AppendLine(@"        ")
				.AppendLine(@"        public bool GetGoalValidity(int target) => blackboard.GetGoalValidity(target);")
				.AppendLine(@"        ")
				.AppendLine(@"        public int GetGoalHeuristic(int target) => blackboard.GetGoalHeuristic(target);")
				.AppendLine(@"        ")
				.AppendLine(@"        public bool GetActionApplicability(int target) => blackboard.GetActionApplicability(target);")
				.AppendLine(@"        ")
				.AppendLine(@"        public void ApplyActionEffect(int target)")
				.AppendLine(@"        {")
				.AppendLine(@"            blackboard.ApplyActionEffect(target);")
				.AppendLine(@"            OnValueUpdate.Invoke();")
				.AppendLine(@"        }")
				.AppendLine(@"        ")
				.AppendLine(@"        public TGoal GetGoal<TGoal>(TGoal[] goals) where TGoal : UnityEngine.IActualGoal => UnityEngine.GoalCalculator.GetGoal(ref blackboard, goals);")
				.AppendLine(@"        ")
				.AppendLine(@"        public TGoal GetGoal<TGoal>(System.Collections.Generic.List<TGoal> goals) where TGoal : UnityEngine.IActualGoal => UnityEngine.GoalCalculator.GetGoal(ref blackboard, goals);")
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
				s += $"            public const int GOAL_UNINITIALIZED = 0;\n";
				
				var collection2Length = Collection2.Length;
				for (i = 0; i < collection2Length; i++)
				{
					s += $"            public const int GOAL_{Collection2[i].Name.PascalToAllUpper()} = {i + 1};\n";
				}

				s += $"            public const int GOAL_COUNT = {i};\n";
				s += $"            public const int ACTION_UNINITIALIZED = 0;\n";

				var collection3Length = Collection3.Length;
				for (i = 0; i < collection3Length; i++)
				{
					s += $"            public const int ACTION_{Collection3[i].Name.PascalToAllUpper()} = {i + 1};\n";
				}

				s += $"            public const int ACTION_COUNT = {i};\n";
				
				return s;
			}
		}

		private string GoalValidities
		{
			get
			{
				var s = "";
				s += $"                ArchetypeIndices.GOAL_UNINITIALIZED => throw new System.Exception(\"Uninitialized goal data received in {name}.\"),\n";

				var collection2Length = Collection2.Length;
				for (var i = 0; i < collection2Length; i++)
				{
					var goal = Collection2[i];
					s += $"                ArchetypeIndices.GOAL_{goal.Name.PascalToAllUpper()} => {goal.ValidityCheck},\n";
				}
				
				s += $"                _ => throw new System.Exception($\"Invalid goal data received by {name}: {{target}}.\")\n";
				return s;
			}
		}

		private string GoalHeuristics
		{
			get
			{
				var s = "";
				s += $"                ArchetypeIndices.GOAL_UNINITIALIZED => throw new System.Exception(\"Uninitialized goal data received by {name}.\"),\n";
				
				var collection2Length = Collection2.Length;
				for (var i = 0; i < collection2Length; i++)
				{
					var goal = Collection2[i];
					s += $"                ArchetypeIndices.GOAL_{goal.Name.PascalToAllUpper()} => {goal.TargetHeuristicString},\n";
				}
				
				s += $"                _ => throw new System.Exception($\"Invalid goal data received by {name}: {{target}}.\")\n";
				return s;
			}
		}

		private string ActionPreconditions
		{
			get
			{
				var s = "";
				s += $"                ArchetypeIndices.ACTION_UNINITIALIZED => throw new System.Exception(\"Uninitialized action data received in {name}.\"),\n";

				var collection3Length = Collection3.Length;
				for (var i = 0; i < collection3Length; i++)
				{
					var action = Collection3[i];
					s += $"                ArchetypeIndices.ACTION_{action.Name.PascalToAllUpper()} => {action.PreconditionCheck},\n";
				}
				
				s += $"                _ => throw new System.Exception($\"Invalid action data received by {name}: {{target}}.\")\n";
				return s;
			}
		}

		private string ActionModifications
		{
			get
			{
				var s = "";
				s += $"                case ArchetypeIndices.ACTION_UNINITIALIZED: throw new System.Exception(\"Uninitialized action data received in {name}.\");\n";

				var collection3Length = Collection3.Length;
				for (var i = 0; i < collection3Length; i++)
				{
					var action = Collection3[i];
					s += $"                case ArchetypeIndices.ACTION_{action.Name.PascalToAllUpper()}:{action.ApplyEffect} break;\n";
				}
				
				s += $"                default: throw new System.Exception($\"Invalid action data received by {name}: {{target}}.\");\n";
				return s;
			}
		}
#endif
	}
}