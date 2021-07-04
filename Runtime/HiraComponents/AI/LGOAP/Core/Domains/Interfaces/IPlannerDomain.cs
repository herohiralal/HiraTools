using HiraEngine.Components.AI.LGOAP.Raw;

namespace HiraEngine.Components.AI.LGOAP
{
    public interface IPlannerDomain
    {
	    Goal[] Goals { get; }
	    byte IntermediateLayerCount { get; }
	    IntermediateGoal[][] IntermediateLayers { get; }
	    Action[] Actions { get; }
		bool IsInitialized { get; }
        RawDomainData DomainData { get; }
    }
}