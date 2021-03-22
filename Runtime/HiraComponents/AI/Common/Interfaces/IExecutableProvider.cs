namespace HiraEngine.Components.AI
{
    public interface IExecutableProvider
    {
        IExecutable GetExecutable(UnityEngine.GameObject target);
    }
}