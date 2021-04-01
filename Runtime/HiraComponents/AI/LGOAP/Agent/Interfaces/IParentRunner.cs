namespace HiraEngine.Components.AI.LGOAP.Internal
{
    public interface IParentLayerRunner
    {
        IChildLayerRunner Child { get; set; }
        ref FlipFlopPool<PlannerResult> Result { get; }
        void OnChildFinished(bool success);
    }

    public interface IChildLayerRunner
    {
        IParentLayerRunner Parent { get; set; }
        void SchedulePlanner();
        MainPlannerJob CreateMainPlannerJob();
        void CollectResult();
    }
}