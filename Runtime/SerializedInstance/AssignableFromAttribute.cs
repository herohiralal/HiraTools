using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AssignableFromAttribute : PropertyAttribute
    {
        public AssignableFromAttribute(Type type) => Type = type;
        public readonly Type Type;
    }
}