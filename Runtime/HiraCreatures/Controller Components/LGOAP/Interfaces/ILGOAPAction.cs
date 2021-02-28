using Unity.Burst;
using Unity.Collections;

namespace UnityEngine
{
	public unsafe delegate byte LGOAPActionApplicabilityCheckDelegate(void* blackboard);

	public unsafe delegate void LGOAPActionModificationDelegate(void* blackboard);
	
	[BurstCompile]
	public readonly struct LGOAPActionData
	{
		public LGOAPActionData(
			float cost,
			FunctionPointer<LGOAPActionApplicabilityCheckDelegate> isApplicableTo,
			FunctionPointer<LGOAPActionModificationDelegate> applyTo)
		{
			Cost = cost;
			IsApplicableTo = isApplicableTo;
			ApplyTo = applyTo;
		}

		[ReadOnly] public readonly float Cost;
		[ReadOnly] public readonly FunctionPointer<LGOAPActionApplicabilityCheckDelegate> IsApplicableTo;
		[ReadOnly] public readonly FunctionPointer<LGOAPActionModificationDelegate> ApplyTo;
	}
	
	public enum LGOAPActionStatus
	{
		Running, Successful, Failed
	}
	
	public interface ILGOAPAction
	{
#if UNITY_EDITOR
		string Name { get; }
#endif
		LGOAPActionData Data { get; }
		void Initialize(GameObject gameObject);
		LGOAPActionStatus Update();
		void Abort();
	}
}