namespace UnityEngine
{
    public class HiraButton : PropertyAttribute
    {
        public HiraButton(string methodName) => MethodName = methodName;
        public string MethodName { get; }
    }
}