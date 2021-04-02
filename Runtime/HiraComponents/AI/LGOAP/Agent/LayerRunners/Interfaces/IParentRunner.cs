namespace HiraEngine.Components.AI.LGOAP.Internal
{
    public interface IParentLayerRunner
    {
        IChildLayerRunner Child { get; set; }
        ref FlipFlopPool<PlannerResult> Result { get; }
        void OnChildFinished();
    }
}