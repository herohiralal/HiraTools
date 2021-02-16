using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HiraButtonAttribute : PropertyAttribute
    {
        public HiraButtonAttribute(string methodName) => MethodName = methodName;
        public string MethodName { get; }
    }
}