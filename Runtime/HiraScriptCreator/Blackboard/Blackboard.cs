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
				.AppendLine($"    public unsafe struct {name}PlannerJob : Unity.Jobs.IJob")
				.AppendLine(@"    {")
				.AppendLine($"        [Unity.Collections.ReadOnly] private readonly int _datasetsLength;")
				.AppendLine($"        [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestriction] private {name}* _datasetsPtr;")
				.AppendLine($"        [Unity.Collections.DeallocateOnJobCompletion] private readonly Unity.Collections.NativeArray<{name}> _datasets;")
				.AppendLine(@"        [Unity.Collections.ReadOnly] private readonly int _goal;")
				.AppendLine($"        [Unity.Collections.ReadOnly] private readonly Unity.Collections.NativeArray<{name}ActionData> _actions;")
				.AppendLine(@"        [Unity.Collections.ReadOnly] private readonly int _actionsCount;")
				.AppendLine(@"        [Unity.Collections.ReadOnly] private readonly float _maxFScore;")
				.AppendLine(@"        [Unity.Collections.WriteOnly] public Unity.Collections.NativeArray<int> Plan;")
				.AppendLine(@"        ")
				.AppendLine($"        public {name}PlannerJob({name}* dataset, int goal, int maxPlanLength, float maxFScore,")
				.AppendLine($"            Unity.Collections.NativeArray<{name}ActionData> actions, Unity.Collections.NativeArray<int> plan)")
				.AppendLine(@"        {")
				.AppendLine($"            _datasets = new Unity.Collections.NativeArray<{name}>(maxPlanLength + 1, Unity.Collections.Allocator.TempJob) {{[0] = *dataset}};")
				.AppendLine($"            _datasetsPtr = ({name}*) Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.GetUnsafePtr(_datasets);")
				.AppendLine(@"            _datasetsLength = maxPlanLength + 1;")
				.AppendLine(@"            _actions = actions;")
				.AppendLine(@"            _actionsCount = actions.Length;")
				.AppendLine(@"            _maxFScore = maxFScore;")
				.AppendLine(@"            _goal = goal;")
				.AppendLine(@"            Plan = plan;")
				.AppendLine(@"        }")
				.AppendLine(@"        ")
				.AppendLine(@"        public void Execute()")
				.AppendLine(@"        {")
				.AppendLine(@"            float threshold = GetHeuristic(_goal, _datasetsPtr), score;")
				.AppendLine(@"            while ((score = PerformHeuristicEstimatedSearch(1, 0, threshold)) < 0 && !(score > _maxFScore) && !(score < 0)) threshold = score;")
				.AppendLine(@"            _datasetsPtr = null;")
				.AppendLine(@"        }")
				.AppendLine(@"        ")
				.AppendLine(@"        private float PerformHeuristicEstimatedSearch(int index, float cost, float threshold)")
				.AppendLine(@"        {")
				.AppendLine(@"            int heuristic;")
				.AppendLine(@"            if ((heuristic = GetHeuristic(_goal, _datasetsPtr + index - 1)) == 0)")
				.AppendLine(@"            {")
				.AppendLine(@"                Plan[0] = index - 1;")
				.AppendLine(@"                return -1;")
				.AppendLine(@"            }")
				.AppendLine(@"            ")
				.AppendLine(@"            var fScore = cost + heuristic;")
				.AppendLine(@"            if (fScore > threshold) return fScore;")
				.AppendLine(@"            ")
				.AppendLine(@"            if (index == _datasetsLength) return float.MaxValue;")
				.AppendLine(@"            ")
				.AppendLine(@"            var min = float.MaxValue;")
				.AppendLine(@"            ")
				.AppendLine(@"            for (var i = 0; i < _actionsCount; i++)")
				.AppendLine(@"            {")
				.AppendLine(@"                var action = _actions[i];")
				.AppendLine(@"                ")
				.AppendLine(@"                if (!PreconditionCheck(action.ArchetypeIndex, _datasetsPtr + index - 1)) continue;")
				.AppendLine(@"                ")
				.AppendLine(@"                *(_datasetsPtr + index) = *(_datasetsPtr + index - 1);")
				.AppendLine(@"                ApplyEffect(action.ArchetypeIndex, _datasetsPtr + index);")
				.AppendLine(@"                ")
				.AppendLine(@"                float score;")
				.AppendLine(@"                if ((score = PerformHeuristicEstimatedSearch(index + 1, cost + action.Cost, threshold)) < 0)")
				.AppendLine(@"                {")
				.AppendLine(@"                    Plan[index] = action.Identifier;")
				.AppendLine(@"                    return -1;")
				.AppendLine(@"                }")
				.AppendLine(@"                ")
				.AppendLine(@"                min = Unity.Mathematics.math.min(score, min);")
				.AppendLine(@"            }")
				.AppendLine(@"            ")
				.AppendLine(@"            return min;")
				.AppendLine(@"        }")
				.AppendLine(@"        ")
				.AppendLine($"        public static int GetHeuristic(int target, {name}* blackboard) =>")
				.AppendLine(@"            target switch")
				.AppendLine(@"            {")
				.Append(GoalHeuristics)
				.AppendLine(@"            };")
				.AppendLine(@"        ")
				.AppendLine($"        public static bool PreconditionCheck(int target, {name}* blackboard) =>")
				.AppendLine(@"            target switch")
				.AppendLine(@"            {")
				.Append(ActionPreconditions)
				.AppendLine(@"            };")
				.AppendLine(@"        ")
				.AppendLine($"        public static void ApplyEffect(int target, {name}* blackboard)")
				.AppendLine(@"        {")
				.AppendLine(@"            switch (target)")
				.AppendLine(@"            {")
				.Append(ActionModifications)
				.AppendLine(@"            }")
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
				
				var collection2Length = Collection2.Length;
				for (i = 0; i < collection2Length; i++)
				{
					s += $"        public const int GOAL_{Collection2[i].Name.PascalToAllUpper()} = {i + 1};\n";
				}

				s += $"        public const int GOAL_COUNT = {i};\n";
				s += "        public const int ACTION_UNINITIALIZED = 0;\n";

				var collection3Length = Collection3.Length;
				for (i = 0; i < collection3Length; i++)
				{
					s += $"        public const int ACTION_{Collection3[i].Name.PascalToAllUpper()} = {i + 1};\n";
				}

				s += $"        public const int ACTION_COUNT = {i};\n";
				
				return s;
			}
		}

		private string GoalHeuristics
		{
			get
			{
				var s = "";
				s += $"                {name}ArchetypeIndices.GOAL_UNINITIALIZED => throw new System.Exception(\"Uninitialized goal data received by {name}PlannerJob.\"),\n";
				
				var collection2Length = Collection2.Length;
				for (var i = 0; i < collection2Length; i++)
				{
					var goal = Collection2[i];
					s += $"                {name}ArchetypeIndices.GOAL_{goal.Name.PascalToAllUpper()} => {goal.TargetHeuristicString},\n";
				}
				
				s += $"                _ => throw new System.Exception($\"Invalid action data received by {name}PlannerJob: {{target}}.\")\n";
				return s;
			}
		}

		private string ActionPreconditions
		{
			get
			{
				var s = "";
				s += $"                {name}ArchetypeIndices.ACTION_UNINITIALIZED => throw new System.Exception(\"Uninitialized action data received in {name}PlannerJob.\"),\n";

				var collection3Length = Collection3.Length;
				for (var i = 0; i < collection3Length; i++)
				{
					var action = Collection3[i];
					s += $"                {name}ArchetypeIndices.ACTION_{action.Name.PascalToAllUpper()} => {action.PreconditionCheck},\n";
				}
				
				s += $"                _ => throw new System.Exception($\"Invalid action data received by {name}PlannerJob: {{target}}.\")\n";
				return s;
			}
		}

		private string ActionModifications
		{
			get
			{
				var s = "";
				s += $"                case {name}ArchetypeIndices.ACTION_UNINITIALIZED: throw new System.Exception(\"Uninitialized action data received in {name}PlannerJob.\");\n";

				var collection3Length = Collection3.Length;
				for (var i = 0; i < collection3Length; i++)
				{
					var action = Collection3[i];
					s += $"                case {name}ArchetypeIndices.ACTION_{action.Name.PascalToAllUpper()}:{action.ApplyEffect} break;\n";
				}
				
				s += $"                default: throw new System.Exception($\"Invalid action data received by {name}PlannerJob: {{target}}.\");\n";
				return s;
			}
		}
#endif
	}
}