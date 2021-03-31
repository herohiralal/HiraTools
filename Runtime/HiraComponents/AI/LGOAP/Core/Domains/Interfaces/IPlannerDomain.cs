using HiraEngine.Components.AI.LGOAP.Raw;

namespace HiraEngine.Components.AI.LGOAP
{
    public interface IPlannerDomain
    {
        RawDomainData DomainData { get; }
    }
}