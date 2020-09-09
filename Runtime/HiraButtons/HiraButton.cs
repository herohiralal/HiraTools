namespace UnityEngine
{
    public class HiraButton : PropertyAttribute
    {
        // TODO: Create a readme for this.
        public HiraButton(string methodName) => MethodName = methodName;
        public string MethodName { get; }
    }
}