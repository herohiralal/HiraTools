using Unity.Burst;
using Unity.Collections;

namespace UnityEngine
{
	public unsafe delegate byte LGOAPGoalValidityCheckDelegate(void* blackboard);
	public unsafe delegate int LGOAPGoalHeuristicCheckDelegate(void* blackboard);

	public readonly struct LGOAPGoalData
	{
		[ReadOnly] public readonly float Insistence;
		[ReadOnly] public readonly FunctionPointer<LGOAPGoalValidityCheckDelegate> IsValidOn;

		public LGOAPGoalData(float insistence, FunctionPointer<LGOAPGoalValidityCheckDelegate> isValidOn)
		{
			Insistence = insistence;
			IsValidOn = isValidOn;
		}
	}

	public interface ILGOAPGoal
	{
#if UNITY_EDITOR
		string Name { get; }
#endif
		LGOAPGoalData Data { get; }
		LGOAPGoalHeuristicCheckDelegate GetHeuristicFor { get; }
	}
}