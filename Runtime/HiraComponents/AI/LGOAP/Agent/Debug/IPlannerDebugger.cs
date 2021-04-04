namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public interface IPlannerDebugger
	{
		void UpdateGoal(byte index);
		void UpdateIntermediatePlan(byte layerIndex, PlannerResult plan);
		void IncrementIntermediateGoalIndex(byte layerIndex);
		void UpdateCorePlan(PlannerResult plan);
		void IncrementActionIndex();
		void UpdateExecutableIndex(byte newIndex);
	}
}