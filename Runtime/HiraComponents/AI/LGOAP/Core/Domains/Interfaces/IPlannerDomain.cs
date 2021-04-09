using HiraEngine.Components.AI.LGOAP.Raw;
using HiraEngine.Components.Blackboard.Internal;

namespace HiraEngine.Components.AI.LGOAP
{
    public interface IPlannerDomain
    {
	    Goal[] Goals { get; }
	    byte IntermediateLayerCount { get; }
	    IntermediateGoal[][] IntermediateLayers { get; }
	    Action[] Actions { get; }
        IBlackboardEffector[] Restarters { get; } 
		bool IsInitialized { get; }
        RawDomainData DomainData { get; }
    }
}