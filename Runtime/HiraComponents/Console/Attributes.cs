using System;

namespace UnityEngine
{
	[AttributeUsage(AttributeTargets.Class)]
	public class HiraConsoleTypeAttribute : Attribute
    {
        public HiraConsoleTypeAttribute(string name = null, bool noDot = false) => (Name, AddDot) = (name, !noDot);

        public readonly string Name;
        public readonly bool AddDot;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HiraConsoleCallableAttribute : Attribute
    {
        public HiraConsoleCallableAttribute(string name = null) => (Name) = (name);

        public readonly string Name;
    }
}