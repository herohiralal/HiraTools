namespace HiraEngine.Components.AI.LGOAP.Internal
{
	public interface IChildLayerRunner
	{
		IPlannerDebugger Debugger { get; set; }
		void SchedulePlanner();
		MainPlannerJob CreateMainPlannerJob();
		void CollectResult();
		bool SelfAndAllChildrenIdle { get; }
		bool SelfOrAnyChildScheduled { get; }
		bool SelfOrAnyChildRunning { get; }
        void IgnoreScheduledPlannerRunForSelfAndChild();
        void IgnorePlannerResultForSelfAndChild();
	}
}